using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Views : MonoBehaviour
{

    TCPMessageHandler _mh;

    int _viewId = 0;

    // Start is called before the first frame update
    void Start()
    {
        _mh = FindObjectOfType<TCPMessageHandler>();
        _mh.AddCallback("Restart", RestartCallback);

        Restart();
    }

    // Update is called once per frame
    void Update()
    {   
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
            transform.GetChild(_viewId).gameObject.SetActive(true);
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
