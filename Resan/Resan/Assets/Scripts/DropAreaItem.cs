using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropAreaItem : MonoBehaviour {

    public PickableItem item;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetItem(PickableItem pi) {
        item = pi;
    }

    public bool HasItem() {
        return item != null;
    }

}
