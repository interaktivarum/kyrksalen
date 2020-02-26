using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase : MonoBehaviour {

    public Views views;
    public bool blockScreensaver = false;
    protected bool _unloading = false;

    public virtual void SetReferences() {
        views = GetComponentInParent<Views>();
    }

    public virtual void LoadView() {
        gameObject.SetActive(true);
        _unloading = false;
    }

    public void InitUnloadView() {
        StartCoroutine(UnloadView());
    }

    IEnumerator UnloadView() {
        yield return DoUnloadView();
        _unloading = true;
        views.NextView();
    }

    public virtual YieldInstruction DoUnloadView() {
        return new YieldInstruction();
    }

    public void SendMessageToServer(JsonMessage msg) {
        views._mh.SendMessageToServer(msg);
    }

    public void SendMessageToServer(JsonMessage msg, TCPMessageHandler.CallbackDelegate cb) {
        views._mh.SendMessageToServer(msg, cb);
    }

    public void SendStringToServer(string str) {
        views._mh.SendStringToServer(str);
    }

}

