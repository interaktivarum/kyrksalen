using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{

    public TCPMessageHandler _mh;

    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = true;
        Screen.orientation = ScreenOrientation.AutoRotation;

        _mh = FindObjectOfType<TCPMessageHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _mh.SendStringToServer("ApplicationQuit");
            Application.Quit();
        }
    }

    private void OnDestroy() {
    }

}
