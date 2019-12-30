using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
