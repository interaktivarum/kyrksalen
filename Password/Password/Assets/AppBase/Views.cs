using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Views : MonoBehaviour
{

    public TCPMessageHandler _mh;

    int _viewId = 0;

    // Start is called before the first frame update
    void Start()
    {
        _mh = FindObjectOfType<TCPMessageHandler>();
        _mh.AddCallback("Restart", RestartCallback);

        SetReferences();

        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            transform.GetChild(_viewId).GetComponentInChildren<ViewBase>().UnloadView();
        }
    }

    void SetReferences() {
        foreach(Transform child in transform) {
            child.gameObject.SetActive(true);
            child.GetComponent<ViewBase>().SetReferences();
        }
    }

    void LoadView(int id) {
        _viewId = id;
        LoadCurrentView();
    }

    void LoadCurrentView() {
        if (_viewId < transform.childCount) {
            foreach (Transform view in transform) {
                view.gameObject.SetActive(false);
            }
            transform.GetChild(_viewId).GetComponentInChildren<ViewBase>().LoadView();
            _mh.SendStringToServer("ViewLoaded:" + transform.GetChild(_viewId).name);
        }
    }

    public void NextView() {
        _viewId = (_viewId + 1) % transform.childCount;
        LoadCurrentView();
    }

    public void RestartCallback(string args) {
        Restart();
    }

    public void Restart() {
        LoadView(0);
    }

}
