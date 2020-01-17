using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropAreaItems : MonoBehaviour {

    public int cols = 3;
    public int rows = 2; 
    //public List<PickableItem> items;
    public ItemSlot[] _slots;

    // Start is called before the first frame update
    void Start() {
        PopulateSlots();
    }

    // Update is called once per frame
    void Update() {
    }

    void PopulateSlots() {
        int nSlots = GetNSlots();
        _slots = new ItemSlot[nSlots];
        for (int i = 0; i < nSlots; i++) {
            _slots[i] = new ItemSlot();
            _slots[i].position = CalcSlotPosition(i);
        }
    }

    Vector3 CalcSlotPosition(int id) {
        Bounds bounds = GetComponent<BoxCollider>().bounds;
        float w = bounds.size.x;
        float h = bounds.size.y;
        int col = id % cols;
        int row = Mathf.FloorToInt(id / cols);
        float x = bounds.min.x + (col + 1) * w / (cols + 1);
        float y = bounds.min.y + (row + 1) * h / (rows + 1);
        return new Vector3(x, y, transform.position.z);
    }

    public ItemSlot AddItem(PickableItem item) {
        ItemSlot slot = GetClosestEmptySlot(item.transform.position);
        if (slot != null) {
            slot.AddItem(item);
        }
        return slot;
    }

    public void RemoveItem(PickableItem item) {
        foreach (ItemSlot slot in _slots) {
            if (slot.item == item) {
                slot.RemoveItem();
            }
        }
    }

    int GetNSlots() {
        return cols * rows;
    }

    public bool HasFreeSlot() {
        return GetEmptySlot() != null;
    }

    public ItemSlot GetEmptySlot() {
        foreach (ItemSlot slot in _slots) {
            if (!slot.item) {
                return slot;
            }
        }
        return null;
    }

    public List<ItemSlot> GetEmptySlots() {
        List<ItemSlot> emptySlots = new List<ItemSlot>();
        foreach (ItemSlot slot in _slots) {
            if (!slot.item) {
                emptySlots.Add(slot);
            }
        }
        return emptySlots;
    }

    public ItemSlot GetClosestEmptySlot(Vector3 pos) {
        List<ItemSlot> emptySlots = GetEmptySlots();
        float distMin = float.MaxValue;
        ItemSlot slotMin = null;
        foreach (ItemSlot slot in emptySlots) {
            float dist = Vector3.SqrMagnitude(pos - slot.position);
            if(dist < distMin) {
                distMin = dist;
                slotMin = slot;
            }
        }
        return slotMin;
    }

}
