using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropAreaWord : MonoBehaviour
{
    ViewLetter _view;

    public bool _solved;

    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponentInParent<ViewLetter>();
    }

    // Update is called once per frame
    void Update()
    {  
    }

    public bool IsSolved() {
        return _solved;
    }

    public void SetSolved(bool state) {
        _solved = state;
        _view.SendStringToServer("WordSolved:"+GetWordId());
        _view.SolvedUpdate();
    }

    int GetWordId() {
        int i = 0;
        foreach(DropAreaWord dropArea in transform.parent.GetComponentsInChildren<DropAreaWord>()) {
            if(dropArea == this) {
                return i;
            }
            i++;
        }
        return -1;
    }

}
