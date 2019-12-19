using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropAreaWord : MonoBehaviour
{

    public bool _solved;

    // Start is called before the first frame update
    void Start()
    { 
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
    }

}
