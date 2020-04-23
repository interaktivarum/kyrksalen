using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRestart : MonoBehaviour
{

    ViewBase _view;

    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponentInParent<ViewBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() {
        Debug.Log("Click restart");
        _view.views.Restart();
    }
}
