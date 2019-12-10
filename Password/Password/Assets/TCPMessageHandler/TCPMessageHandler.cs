using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IPSettings {
    public string ip;
    public int port;
}

[System.Serializable]
public class JsonMessage {

    public string name;
    public int cbInt = -1;
    public string cbStr;
    public string args;

    public JsonMessage() {
    }

    public JsonMessage (string name) {
        this.name = name;
    }

}

[System.Serializable]
public class MsgEvent : UnityEvent<JsonMessage> {
}

[System.Serializable]
public class StrEvent : UnityEvent<string> {
}

public class TCPMessageHandler : MonoBehaviour
{
    public bool autoConnect = false;
    Client _client;
    Server _server;
    IPSettings settings;

    public delegate void CallbackDelegate(string msg); // This defines what type of method you're going to call.

    List<CallbackDelegate> _callbacksIntId = new List<CallbackDelegate>();
    Dictionary<string,List<CallbackDelegate>> _callbacksStrId = new Dictionary<string,List<CallbackDelegate>>();

    public StrEvent _ClientMessageSelfEvent = new StrEvent();
    public MsgEvent _ClientMessageSentEvent = new MsgEvent();
    public MsgEvent _ClientMessageReceivedEvent = new MsgEvent();
    public StrEvent _ServerMessageSelfEvent = new StrEvent();
    public MsgEvent _ServerMessageSentEvent = new MsgEvent();
    public MsgEvent _ServerMessageReceivedEvent = new MsgEvent();

    private void Awake() {
        SettingsFromJSON();
        _client = GetComponentInChildren<Client>();
        _client.SetIPSettings(settings.ip, settings.port);
        _client.SetMessageHandler(this);

        _server = GetComponentInChildren<Server>();
        if (_server) {
            _server.SetMessageHandler(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NetworkCoroutine());
    }

    // Update is called once per frame
    void Update() {
    }

    private IEnumerator NetworkCoroutine() {
        while (true) {
            if (!_client.IsConnected()) {
                if (autoConnect) {
                    
                    StartClient();
                }
            }
            else {
                _ClientMessageSelfEvent.Invoke("Handler: Ping server");
                SendMessageToServer(new JsonMessage("ping"));
            }
            yield return new WaitForSeconds(10);
        }
    }

    public void SetAutoConnect(bool value) {
        autoConnect = value;
    }

    public void AddClientMessageSelfListener(UnityAction<string> call) {
        _ClientMessageSelfEvent.AddListener(call);
    }

    public void AddClientMessageSentListener(UnityAction<JsonMessage> call) {
        _ClientMessageSentEvent.AddListener(call);
    }

    public void AddClientMessageReceivedListener(UnityAction<JsonMessage> call) {
        _ClientMessageReceivedEvent.AddListener(call);
    }

    public void AddServerMessageSelfListener(UnityAction<string> call) {
        _ServerMessageSelfEvent.AddListener(call);
    }

    public void AddServerMessageSentListener(UnityAction<JsonMessage> call) {
        _ServerMessageSentEvent.AddListener(call);
    }

    public void AddServerMessageReceivedListener(UnityAction<JsonMessage> call) {
        _ServerMessageReceivedEvent.AddListener(call);
    }

    public void StartClient() {
        _ClientMessageSelfEvent.Invoke("Handler: Try to connect");
        if (_client) {
            _client.StartClient();
        }
    }

    void SettingsFromJSON() {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Application.streamingAssetsPath + "/ip-settings.json";

        if (File.Exists(filePath)) {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            settings = JsonUtility.FromJson<IPSettings>(dataAsJson);
        }
        else {
            Debug.LogError("Cannot load json data!");
        }
    }

    #region ClientToServer
    //public void SendMessageToServer(string msg, CallbackDelegate cb) {
    public void SendMessageToServer(JsonMessage msg, CallbackDelegate cb) {
        _callbacksIntId.Add(cb);
        msg.cbInt = _callbacksIntId.Count - 1;
        SendMessageToServer(msg);
    }

    public void SendMessageToServer(JsonMessage msg) {
        if (_client.IsConnected()) {
            _client.SendMessageToServer(msg);
        }
    }

    public void RunCallbackIntId(int id, string args) {
        if(id >= 0) {
            _callbacksIntId[id](args);
        }
    }

    public void AddCallback(string msg, CallbackDelegate cb) {
        if (!_callbacksStrId.ContainsKey(msg)) {
            _callbacksStrId.Add(msg, new List<CallbackDelegate>());
        }
        _callbacksStrId[msg].Add(cb);
    }

    public void RunCallbackStrId(string func, string args) {
        if (_callbacksStrId.ContainsKey(func)) {
            foreach (CallbackDelegate cb in _callbacksStrId[func]) {
                cb(args);
            }
        }
    }
    #endregion

    #region ServerToClient
    public void SendMessageToClient(JsonMessage msg) {
        _server.SendMessage(msg);
    }

    #endregion

}
