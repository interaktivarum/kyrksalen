using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Server : MonoBehaviour
{
    #region Public Variables
    [Header("Network")]
    public int port = 8080;
    public float waitingMessagesFrequency = 1;
    #endregion

    #region  Network m_Variables
    private TcpListener m_server = null;
    private TcpClient m_client = null;
    private NetworkStream m_netStream = null;
    private byte[] m_buffer = new byte[49152];
    private int m_bytesReceived = 0;
    private string m_receivedMessage = "";
    private IEnumerator m_ClientComCoroutine = null;
    #endregion

    TCPMessageHandler _messageHandler;

    //Set UI interactable properties
    private void Start()
    {
    }

    public void SetMessageHandler(TCPMessageHandler handler) {
        _messageHandler = handler;
    }

    //Start server and wait for clients
    public void StartServer()
    {
        //Set and enable Server 
        IPAddress ip = IPAddress.Parse(GetLocalIPAddress());
        m_server = new TcpListener(ip, port);
        m_server.Start();
        //Wait for async client connection 
        m_server.BeginAcceptTcpClient(ClientConnected, null);

        _messageHandler._ServerMessageSelfEvent.Invoke("ServerStart");
        _messageHandler._ServerMessageSelfEvent.Invoke("IP: " + GetLocalIPAddress());
    }

    //Check if any client trys to connect
    private void Update()
    {   
        //If some client stablish connection
        if (m_client != null && m_ClientComCoroutine == null)
        {
            //Start the ClientCommunication coroutine
            m_ClientComCoroutine = ClientCommunication();
            StartCoroutine(m_ClientComCoroutine);
        }
    }

    //Coroutine that manage client communication while client is connected to the server

    private IEnumerator ClientCommunication() {

        //Restart values in case there are more than one client connections
        m_bytesReceived = 0;
        m_buffer = new byte[49152];

        //Stablish Client NetworkStream information
        m_netStream = m_client.GetStream();

        _messageHandler._ServerMessageSelfEvent.Invoke("ClientConnected");

        //While there is a connection with the client, await for messages
        do {
            //Start Async Reading
            m_netStream.BeginRead(m_buffer, 0, m_buffer.Length, MessageReceived, m_netStream);

            //If there is any msg
            if (m_bytesReceived > 0) {

                JsonMessage msg = JsonUtility.FromJson<JsonMessage>(m_receivedMessage);

                if (msg.name == "MouseButtonLeft") {
                    SendMessage(msg);
                }
                else if (msg.name == "MouseButtonRight") {
                    msg.cbStr = "SetColors";
                    msg.args = "{\"r\": 1, \"g\": 1, \"b\": 1}";
                    SendMessage(msg);
                }
                //If message received from client is "Close", send another "Close" to the client
                else if (msg.name == "Close") {
                    JsonMessage msgClose = new JsonMessage {
                        cbStr = "Close"
                    };
                    SendMessage(msgClose);
                    m_netStream = null;
                }

                _messageHandler._ServerMessageReceivedEvent.Invoke(msg);
            }

            m_bytesReceived = 0;

            yield return new WaitForSeconds(waitingMessagesFrequency);

        } while (m_bytesReceived >= 0 && m_netStream != null && m_client.Connected);

        //The communication is over
        CloseConnection();
        _messageHandler._ServerMessageSelfEvent.Invoke("ClientDisconnected");
    }

    public void SendMessage(JsonMessage msg) {

        string msgStr = JsonUtility.ToJson(msg);

        byte[] msgBytes = Encoding.ASCII.GetBytes(msgStr); //Encode message as bytes
        m_netStream.Write(msgBytes, 0, msgBytes.Length);

        _messageHandler._ServerMessageSentEvent.Invoke(msg);
    }

    //Callback called when "BeginRead" is ended
    private void MessageReceived(IAsyncResult result)
    {
        if (result.IsCompleted && m_client.Connected)
        {
            //build message received from client
            m_bytesReceived = m_netStream.EndRead(result);                              //End async reading
            m_receivedMessage = Encoding.ASCII.GetString(m_buffer, 0, m_bytesReceived);   //De-encode message as string
        }
    }

    //Callback called when "BeginAcceptTcpClient" detects new client connection
    private void ClientConnected(IAsyncResult res)
    {
        //set the client reference
        m_client = m_server.EndAcceptTcpClient(res);
    }

    //Close connection with the client
    private void CloseConnection()
    {

        if (m_ClientComCoroutine != null) {
            StopCoroutine(m_ClientComCoroutine);
            m_ClientComCoroutine = null;
        }

        if (m_client != null) {
            m_client.Close();
            m_client = null;
        }

        //Waiting to Accept a new Client
        if (m_server != null) {
            m_server.BeginAcceptTcpClient(ClientConnected, null);
        }

        

    }    

    //Close client connection and disables the server
    public void CloseServer()
    {

        CloseConnection();

        //Close server connection
        if(m_server != null)
        {
            m_server.Stop();
            m_server = null;
        }

        _messageHandler._ServerMessageSelfEvent.Invoke("ServerClose");
    }

    public static string GetLocalIPAddress() {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList) {
            if (ip.AddressFamily == AddressFamily.InterNetwork) {
                return ip.ToString();
            }
        }
        throw new Exception("IP adress kunde ej hämtas");
    }

}