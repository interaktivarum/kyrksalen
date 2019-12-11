using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ClientHandler : MonoBehaviour
{

    public TCPMessageHandler _mh;

    [Header("UI References")]
    public Button sendConnectButton;
    public Button sendCloseButton;
    public Text Logger = null;

    private void Awake() {
        _mh.AddClientMessageSelfListener(LogSelf);
        _mh.AddClientMessageSentListener(LogSent);
        _mh.AddClientStringSentListener(LogSent);
        _mh.AddClientMessageReceivedListener(LogReceived);
        _mh.AddClientStringReceivedListener(LogReceived);

        Log("Klientlogg, nya inlägg överst");

    }

    // Start is called before the first frame update
    void Start()
    {
        sendConnectButton.interactable = true;
        sendCloseButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {   
    }

    public void StartClient() {
        _mh.StartClient();
    }

    //Custom Log
    #region Log
    private void Log(string msg, Color color) {
        Logger.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + msg + "</color>" + '\n' + Logger.text;
    }
    private void Log(string msg) {
        Logger.text = msg + '\n' + Logger.text;
    }

    private void LogSelf(string msg) {
        if (msg == "ConnectedToServer") {
            sendConnectButton.interactable = false;
            sendCloseButton.interactable = true;
        }
        else if (msg == "DisconnectedFromServer") {
            sendConnectButton.interactable = true;
            sendCloseButton.interactable = false;
        }
        Log(msg);
    }

    private void LogSent(JsonMessage msg) {
        Log("Skickat: " + JsonUtility.ToJson(msg), Color.blue);
    }

    private void LogSent(string str) {
        Log("Skickat: " + str, Color.blue);
    }

    private void LogReceived(JsonMessage msg) {
        Log("Mottaget: " + JsonUtility.ToJson(msg), new Color(0.6f, 0f, 0.3f));
    }

    private void LogReceived(string str) {
        Log("Mottaget: " + str, new Color(0.6f, 0f, 0.3f));
    }
    #endregion

}
