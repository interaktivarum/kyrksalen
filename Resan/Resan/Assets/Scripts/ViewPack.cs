using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPack : ViewBase
{

    DropAreaItems _dropArea;

    // Start is called before the first frame update
    void Start()
    {
        _dropArea = GetComponentInChildren<DropAreaItems>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemPacked() {
        if (!_dropArea.HasFreeSlot()) {
            SendStringToServer("AllItemsPacked:1");
            UnloadView();
        }
    }
}
