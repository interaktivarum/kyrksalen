using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ServerHandler : MonoBehaviour {

    public TCPMessageHandler _mh;

    [Header("UI References")]
    public Button startServerButton;
    public Button closeServerButton;
    public Button sendCustomMessageButton;
    public Button clearCustomMessageButton;
    public InputField customMessage;
    public Text Logger = null;

    // Start is called before the first frame update
    void Start() {
        closeServerButton.interactable = false;
        sendCustomMessageButton.interactable = false;
        _mh.AddServerMessageSelfListener(LogSelf);
        _mh.AddServerMessageSentListener(LogSent);
        _mh.AddServerMessageReceivedListener(LogReceived);

        ClearCustomMessage();

        Log("Serverlogg, nya inlägg överst");
    }

    // Update is called once per frame
    void Update() {
    }

    public void StartClient() {
        _mh.StartClient();
    }

    public void SendFixedMessage() {
        JsonMessage msg = new JsonMessage {
            cbStr = "RandomizeColors"
        };
        _mh.SendMessageToClient(msg);
    }

    public void SendCustomMessage() {
        _mh.SendMessageToClient(JsonUtility.FromJson<JsonMessage>(customMessage.text));
    }

    public void ClearCustomMessage() {
        customMessage.text = JsonUtility.ToJson(new JsonMessage {
            cbStr = "RandomizeColors"
        });
    }

    //Custom Log
    #region Log
    private void Log(string msg, Color color) {
        Logger.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + msg + "</color>" + '\n' + Logger.text; ;
    }

    private void Log(string msg) {
        Logger.text = msg + '\n' + Logger.text;
    }

    private void LogSelf(string msg) {
        if(msg == "ServerStart") {
            startServerButton.interactable = false;
            closeServerButton.interactable = true;
        }
        else if (msg == "ServerClose") {
            startServerButton.interactable = true;
            closeServerButton.interactable = false;
            sendCustomMessageButton.interactable = false;
        }
        else if (msg == "ClientConnected") {
            sendCustomMessageButton.interactable = true;
        }
        else if (msg == "ClientDisconnected") {
            sendCustomMessageButton.interactable = false;
        }
        Log(msg);
    }

    private void LogSent(JsonMessage msg) {
        Log("Skickat: " + JsonUtility.ToJson(msg), Color.blue);
    }

    private void LogReceived(JsonMessage msg) {
        Log("Mottaget: " + JsonUtility.ToJson(msg), new Color(0.6f,0f,0.3f));
    }
    #endregion

}
