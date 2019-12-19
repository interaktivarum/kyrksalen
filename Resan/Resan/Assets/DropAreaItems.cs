using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropAreaItems : MonoBehaviour {

    public PickableItem[] items;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public Vector3 AddItem(PickableItem pi) {
        return transform.localPosition;
    }

    public void RemoveItem(PickableItem pi) {
    }

    public bool HasFreeSlot() {
        return true;
    }

}
