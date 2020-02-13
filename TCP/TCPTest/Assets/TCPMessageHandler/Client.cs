using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour {
    #region Public Variables
    [Header("Network")]
    public string ipAddress = "127.0.0.1";
    public int port = 54010;
    public float waitingMessagesFrequency = 1;
    #endregion

    #region Network m_Variables
    private TcpClient m_client;
    private NetworkStream m_netStream = null;
    private byte[] m_buffer = new byte[49152];
    private int m_bytesReceived = 0;
    private string m_receivedMessage = "";
    private IEnumerator m_ServerComCoroutine = null;
    bool _isReading;
    Task _connectTask = null;
    #endregion

    TCPMessageHandler _messageHandler;


    //Set UI interactable properties
    private void Start() {
    }

    public void SetMessageHandler(TCPMessageHandler handler) {
        _messageHandler = handler;
        _messageHandler.AddCallback("CloseConnection", CloseClient);
        _messageHandler.AddCallback("Quit", Quit);
    }

    public void SetIPSettings(string ip, int p) {
        ipAddress = ip;
        port = p;
    }

    //Start client and stablish connection with server
    public void StartClient() {

        //Early out
        /*if (m_client != null) {
            return;
        }*/

        //Create new client
        if (m_client == null) {
            m_client = new TcpClient();
        }

        _messageHandler._ClientMessageSelfEvent.Invoke("Try to connect to server");
        _connectTask = m_client.ConnectAsync(ipAddress, port);

        /*try {
            //Set and enable client
            m_client.Connect(ipAddress, port);
            _messageHandler._ClientMessageSelfEvent.Invoke("Connected to server");
        }
        catch (SocketException) {
            Debug.Log("Fel");
            CloseConnection();
            m_client = null;
            _messageHandler._ClientMessageSelfEvent.Invoke("Could not connect to server");
        }*/
        
    }

    //Check if the client has been recived something
    private void Update() {

        if(_connectTask != null && _connectTask.IsCompleted) {
            if(_connectTask.Status != TaskStatus.Faulted){
                if (m_client != null && m_ServerComCoroutine == null) {

                    _messageHandler._ClientMessageSelfEvent.Invoke("Connected to server");

                    //Start the ClientCommunication coroutine
                    m_ServerComCoroutine = ServerCommunication();
                    StartCoroutine(m_ServerComCoroutine);
                }
            }
            else {
                _messageHandler._ClientMessageSelfEvent.Invoke("Could not connect to server");
            }
            _connectTask = null;
        }       

    }

    public bool IsConnected() {
        return m_client != null && m_client.Connected;
    }

    //Coroutine that manage client communication while client is connected to the server

    private IEnumerator ServerCommunication() {

        //Restart values in case there are more than one client connections
        m_bytesReceived = 0;
        m_buffer = new byte[49152];

        //Stablish Client NetworkStream information
        m_netStream = m_client.GetStream();
        //While there is a connection with the client, await for messages

        _messageHandler._ClientMessageSelfEvent.Invoke("ConnectedToServer");
        _messageHandler.SendStringToServer("ClientConnected");

        _isReading = false;

        do {
            if (!_isReading) {
                _isReading = true;
                //Start Async Reading
                m_netStream.BeginRead(m_buffer, 0, m_buffer.Length, MessageReceived, m_netStream);
            }

            //If there is any msg
            if (m_bytesReceived > 0) {

                ReadMessage();
                m_bytesReceived = 0;
                _isReading = false;

                //m_receivedMessage = "";
            }

            yield return new WaitForSeconds(1);

        } while (m_bytesReceived >= 0 && m_netStream != null && m_client.Connected);

        CloseConnection();
        _messageHandler._ClientMessageSelfEvent.Invoke("DisconnectedFromServer");
    }

    void ReadMessage() {
        bool isJsonMessage = false;

        try {
            JsonMessage msg = JsonUtility.FromJson<JsonMessage>(m_receivedMessage);

            if (msg.cbInt >= 0) {
                _messageHandler.RunCallbackIntId(msg.cbInt, msg.args);
            }
            if (msg.cbStr.Length > 0) {
                _messageHandler.RunCallbackStrId(msg.cbStr, msg.args);
            }

            _messageHandler._ClientMessageReceivedEvent.Invoke(msg);

            isJsonMessage = true;
        }
        catch (Exception e) {
        }

        if (!isJsonMessage) {

            string[] msgs = m_receivedMessage.Trim().Split(_messageHandler.separator);

            foreach(string msg in msgs) {
                string msgTrim = msg.Trim(_messageHandler.appendString);
                msgTrim = msgTrim.Trim(_messageHandler.prependString);
                _messageHandler.RunCallbackStrId(msgTrim, null);
                _messageHandler._ClientStringReceivedEvent.Invoke(msg);
            }
            
        }
    }
        
    //Send "Close" message to the server, and waits the "Close" message response from server
    public void SendCloseToServer() {
        _messageHandler.SendStringToServer("CloseConnection");
    }

    public void SendMessageToServer(JsonMessage msg) {
        if (!m_client.Connected || m_netStream == null) return; //early out if there is nothing connected
        DoSendString(JsonUtility.ToJson(msg));
        _messageHandler._ClientMessageSentEvent.Invoke(msg);
    }

    public void SendStringToServer(string str) {
        if (!m_client.Connected || m_netStream == null) return; //early out if there is nothing connected
        DoSendString(str);
        _messageHandler._ClientStringSentEvent.Invoke(str);
    }

    void DoSendString(string str) {
        //Build message to server
        byte[] msgBytes = Encoding.UTF8.GetBytes(str + _messageHandler.separator);
        //Start Sync Writing
        try {
            m_netStream.Write(msgBytes, 0, msgBytes.Length);
        }
        catch(Exception e) { }
    }

    //Callback called when "BeginRead" is ended
    private void MessageReceived(IAsyncResult result) {
        if (result.IsCompleted && m_client.Connected) {
            //build message received from server
            m_bytesReceived = m_netStream.EndRead(result);
            m_receivedMessage = Encoding.UTF8.GetString(m_buffer, 0, m_bytesReceived);
        }
    }

    //Close client connection
    private void CloseClient(string msg) {
        m_netStream = null;
    }

    private void Quit(string msg) {
        Application.Quit();
    }

    public void CloseConnection() {

        if (m_ServerComCoroutine != null) {
            StopCoroutine(m_ServerComCoroutine);
            m_ServerComCoroutine = null;
        }

        if (m_client != null) {
            //Reset everything to defaults
            m_client.Close();
            m_client = null;
            _messageHandler._ClientMessageSelfEvent.Invoke("Closing client connection to server");
        }

    }

    void OnApplicationQuit() {
        if (IsConnected()) {
            SendCloseToServer();
        }
    }
}