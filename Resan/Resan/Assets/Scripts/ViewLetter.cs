using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewLetter : ViewBase
{

    DropAreaWord[] _dropAreas;

    // Start is called before the first frame update
    void Start()
    {
        _dropAreas = GetComponentsInChildren<DropAreaWord>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SolvedUpdate() {
        bool allSolved = true;
        foreach(DropAreaWord dropArea in _dropAreas) {
            if (!dropArea._solved) {
                allSolved = false;
                break;
            }
        }
        if (allSolved) {
            SendStringToServer("AllWordsSolved:1");
            InitUnloadView();
        }
    }

    public override YieldInstruction DoUnloadView() {
        return new WaitForSeconds(2);
    }

}
