using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{

    public TCPMessageHandler _mh;
    public Views _views;
    float _timeLastInteraction;

    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = true;
        Screen.orientation = ScreenOrientation.AutoRotation;

        InteractionUpdate();

        _mh = FindObjectOfType<TCPMessageHandler>();
        _views = FindObjectOfType<Views>();

    }

    // Update is called once per frame
    void Update() {
        InteractionUpdate();
        RestartTest();
        QuitTest();
    }

    void InteractionUpdate() {
        if (Input.anyKey || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {
            SetInteraction();
        }
    }

    public void SetInteraction() {
        _timeLastInteraction = Time.fixedTime;
    }

    void RestartTest() {
        if (Time.fixedTime - _timeLastInteraction > 60) {
            SetInteraction();
            _views.RestartScreensaver();
        }
    }

    void QuitTest() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _mh.SendStringToServer("ApplicationQuit");
            Application.Quit();
        }
    }

    private void OnDestroy() {
    }

}
