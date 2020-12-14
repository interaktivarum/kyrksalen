using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ViewBase : MonoBehaviour {

    public Views views;
    public bool blockScreensaver = false;
    protected bool _unloading = false;
    protected List<Sequence> _sequences = new List<Sequence>();
    public float fadeDurationView = 2f;

    public virtual void SetReferences() {
        views = GetComponentInParent<Views>();
    }

    public virtual void LoadView() {
        gameObject.SetActive(true);
        _unloading = false;
    }

    public virtual void InitUnloadView() {
        StartCoroutine(UnloadView(-1));
    }

    public virtual void InitUnloadView(int idView) {
        StartCoroutine(UnloadView(idView));
    }

    IEnumerator UnloadView(int idView) {
        yield return DoUnloadView();
        KillSequences();
        _unloading = true;
        if(idView >= 0) {
            views.FadeToView(idView);
        }
        else {
            views.NextView();
        }
    }

    public virtual YieldInstruction DoUnloadView() {
        return new YieldInstruction();
    }

    protected void AddSequence(Sequence seq) {
        _sequences.Add(seq);
    }

    void KillSequences() {
        foreach (Sequence seq in _sequences) {
            seq.Kill();
        }
        _sequences.Clear();
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

