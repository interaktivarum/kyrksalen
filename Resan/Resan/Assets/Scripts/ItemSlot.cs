using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlot
{
    public int id;
    public PickableItem item = null;
    public Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(PickableItem i) {
        item = i;
    }

    public void RemoveItem() {
        item = null;
    }

    public bool IsEmpty() {
        return item == null;
    }

}
