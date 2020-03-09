using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPack : ViewBase
{

    DropAreaItems _dropArea;
    int _sortingOrder = 0;

    // Start is called before the first frame update
    void Start()
    {
        _dropArea = GetComponentInChildren<DropAreaItems>();
    }

    private void OnEnable() {
        _sortingOrder = 0;
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public void OnItemPacked() {
        if (!_dropArea.HasFreeSlot()) {
            SendStringToServer("AllItemsPacked:1");
            InitUnloadView();
        }
    }

    public override YieldInstruction DoUnloadView() {
        return new WaitForSeconds(2);
    }

    public int NextSortingOrder() {
        return ++_sortingOrder;
    }

}
