using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
    #endregion

    TCPMessageHandler _messageHandler;

    //Set UI interactable properties
    private void Start() {
    }

    public void SetMessageHandler(TCPMessageHandler handler) {
        _messageHandler = handler;
        _messageHandler.AddCallback("Close", CloseClient);
    }

    public void SetIPSettings(string ip, int p) {
        ipAddress = ip;
        port = p;
    }

    //Start client and stablish connection with server
    public void StartClient() {

        //Early out
        if (m_client != null) {
            return;
        }

        _messageHandler._ClientMessageSelfEvent.Invoke("Try to connect to server");

        try {
            //Create new client
            m_client = new TcpClient();
            //Set and enable client
            m_client.Connect(ipAddress, port);
            _messageHandler._ClientMessageSelfEvent.Invoke("Connected to server");
        }
        catch (SocketException) {
            CloseConnection();
            m_client = null;
            _messageHandler._ClientMessageSelfEvent.Invoke("Could not connect to server");
        }
    }

    //Check if the client has been recived something
    private void Update() {

        //If some client stablish connection
        if (m_client != null && m_ServerComCoroutine == null) {
            //Start the ClientCommunication coroutine
            m_ServerComCoroutine = ServerCommunication();
            StartCoroutine(m_ServerComCoroutine);            
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
        SendMessageToServer(new JsonMessage("ClientConnected"));

        do {
            //Start Async Reading
            m_netStream.BeginRead(m_buffer, 0, m_buffer.Length, MessageReceived, m_netStream);

            //If there is any msg
            if (m_bytesReceived > 0) {

                JsonMessage msg = JsonUtility.FromJson<JsonMessage>(m_receivedMessage);

                if (msg.cbInt >= 0) {
                    _messageHandler.RunCallbackIntId(msg.cbInt, msg.args);
                }
                if (msg.cbStr.Length > 0) {
                    _messageHandler.RunCallbackStrId(msg.cbStr, msg.args);
                }

                _messageHandler._ClientMessageReceivedEvent.Invoke(msg);

                m_receivedMessage = "";
            }

            m_bytesReceived = 0;

            yield return new WaitForSeconds(waitingMessagesFrequency);

        } while (m_bytesReceived >= 0 && m_netStream != null && m_client.Connected);

        CloseConnection();
        _messageHandler._ClientMessageSelfEvent.Invoke("DisconnectedFromServer");
    }
        
    //Send "Close" message to the server, and waits the "Close" message response from server
    public void SendCloseToServer() {
        SendMessageToServer(new JsonMessage("Close"));
    }

    public void SendMessageToServer(JsonMessage msg) {
        if (!m_client.Connected || m_netStream == null) return; //early out if there is nothing connected

        string msgStr = JsonUtility.ToJson(msg);

        //Build message to server
        byte[] msgBytes = Encoding.ASCII.GetBytes(msgStr);
        //Start Sync Writing
        m_netStream.Write(msgBytes, 0, msgBytes.Length);
        _messageHandler._ClientMessageSentEvent.Invoke(msg);
    }

    //Callback called when "BeginRead" is ended
    private void MessageReceived(IAsyncResult result) {
        if (result.IsCompleted && m_client.Connected) {
            //build message received from server
            m_bytesReceived = m_netStream.EndRead(result);
            m_receivedMessage = Encoding.ASCII.GetString(m_buffer, 0, m_bytesReceived);
        }
    }

    //Close client connection
    private void CloseClient(string msg) {
        m_netStream = null;
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