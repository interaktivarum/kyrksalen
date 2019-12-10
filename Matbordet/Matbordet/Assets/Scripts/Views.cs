using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Views : MonoBehaviour
{

    public GameObject[] views;
    public int _viewId;

    // Start is called before the first frame update
    void Start()
    {
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
        foreach (Transform view in transform) {
            view.gameObject.SetActive(false);
        }
        transform.GetChild(_viewId).gameObject.SetActive(true);
    }

    public void NextView() {
        _viewId = (_viewId + 1) % views.Length;
        LoadCurrentView();
    }

    public void Restart() {
        LoadView(0);
    }

}
