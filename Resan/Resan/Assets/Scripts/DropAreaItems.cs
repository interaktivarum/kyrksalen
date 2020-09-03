using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropAreaItems : MonoBehaviour {

    ViewPack _view;

    public int cols = 3;
    public int rows = 2;
    float _wSlot;
    float _hSlot;
    //public List<PickableItem> items;
    public ItemSlot[] _slots;

    // Start is called before the first frame update
    void Start() {
        _view = GetComponentInParent<ViewPack>();
        //PopulateSlots();
    }

    private void OnEnable() {
        foreach (ItemSlot slot in _slots) {
            slot.RemoveItem();
        }
        
    }

    // Update is called once per frame
    void Update() {
    }

    public void PopulateSlots() {
        int nSlots = GetNSlots();
        _slots = new ItemSlot[nSlots];
        for (int i = 0; i < nSlots; i++) {
            _slots[i] = new ItemSlot();
            _slots[i].id = i;
            _slots[i].position = CalcSlotPosition(i);
        }
    }

    Vector3 CalcSlotPosition(int id) {
        Bounds bounds = GetComponent<BoxCollider>().bounds;
        float w = bounds.size.x;
        float h = bounds.size.y;
        _wSlot = w / cols;
        _hSlot = h / rows;
        int col = id % cols;
        int row = Mathf.FloorToInt(id / cols);
        //float x = bounds.min.x + (col + 1) * w / (cols + 1);
        //float y = bounds.min.y + (row + 1) * h / (rows + 1);
        float x = bounds.min.x + (col + 0.5f) * _wSlot;
        float y = bounds.min.y + (row + 0.5f) * _hSlot;
        return new Vector3(x, y, transform.position.z);
    }

    public List<ItemSlot> AddItem(PickableItem item) {
        Vector3 pos = item.transform.position;
        pos.x -= (item.dimensions.x - 1) * _wSlot / 2;
        pos.y -= (item.dimensions.y - 1) * _hSlot / 2;
        ItemSlot slot = GetClosestSlot(pos);
        ItemSlot slotFit = CheckItemFit(slot, item);
        if(slotFit == null) {
            slotFit = CheckItemFit(GetEmptySlots(), item);
        }
        if (slotFit != null) {
            _view.SendStringToServer("ItemPacked:" + item.name);
            List<ItemSlot> slots = AddItemToSlots(slotFit, item);
            //slotFit.AddItem(item);
            return slots;
        }
        return null;
    }

    ItemSlot CheckItemFit(ItemSlot slot, PickableItem item) {
        Vector2 slotCoords = GetSlotCoordinates(slot);
        //Debug.Log("Drop: " + slotCoords.x + "," + slotCoords.y);
        for (int yItem = 0; yItem < item.dimensions.y; yItem++) {
            for (int xItem = 0; xItem < item.dimensions.x; xItem++) {
                Vector2 coords = slotCoords - new Vector2(xItem, yItem);
                bool fits = CheckItemFit(coords, item);
                //bool fits = CheckItemFit(slotCoords, item);
                if (fits) {
                    return GetSlot((int)coords.x, (int)coords.y);
                }
            }
        }
        return null;       
    }

    ItemSlot CheckItemFit(List<ItemSlot> slots, PickableItem item) {
        foreach(ItemSlot slot in slots) {
            bool fits = CheckItemFit(GetSlotCoordinates(slot), item);
            //bool fits = CheckItemFit(slotCoords, item);
            if (fits) {
                return slot;
            }
        }
        return null;
    }

    bool CheckItemFit(Vector2 slotCoords, PickableItem item) {
        for (int yItem = 0; yItem < item.dimensions.y; yItem++) {
            for (int xItem = 0; xItem < item.dimensions.x; xItem++) {
                int x = (int)slotCoords.x + xItem;
                int y = (int)slotCoords.y + yItem;
                //Debug.Log(x + "," + y + " [" + (cols * y + x) + "]: " + GetSlot(x,y).IsEmpty());
                ItemSlot slot = GetSlot(x, y);
                if (slot == null/* || !slot.IsEmpty()*/) {
                    return false;
                }
            }
        }
        return true;
    }

    List<ItemSlot> AddItemToSlots(ItemSlot slot, PickableItem item) {
        List<ItemSlot> slots = new List<ItemSlot>();
        Vector2 slotCoords = GetSlotCoordinates(slot);
        for (int yItem = 0; yItem < item.dimensions.y; yItem++) {
            for (int xItem = 0; xItem < item.dimensions.x; xItem++) {
                ItemSlot slotCovered = GetSlot((int)slotCoords.x + xItem, (int)slotCoords.y + yItem);
                slotCovered.AddItem(item);
                slots.Add(slotCovered);
            }
        }
        return slots;
    }

    public ItemSlot GetSlot(int x, int y) {
        if(x >= 0 && x < cols && y >= 0 && y < rows) {
            return _slots[cols * y + x];
        }
        return null;
    }

    public Vector2 GetSlotCoordinates(ItemSlot slot) {
        return new Vector2(slot.id % cols, Mathf.FloorToInt(slot.id / cols));
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

    public List<ItemSlot> GetAllSlots() {
        List<ItemSlot> slots = new List<ItemSlot>();
        foreach (ItemSlot slot in _slots) {
            slots.Add(slot);
        }
        return slots;
    }

    public ItemSlot GetClosestSlot(Vector3 pos, bool empty = false) {
        List<ItemSlot> slots = empty ? GetEmptySlots() : GetAllSlots();
        float distMin = float.MaxValue;
        ItemSlot slotMin = null;
        foreach (ItemSlot slot in slots) {
            float dist = Vector3.SqrMagnitude(pos - slot.position);
            if (dist < distMin) {
                distMin = dist;
                slotMin = slot;
            }
        }
        return slotMin;
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

    public void LockItems() {
        foreach (ItemSlot slot in _slots) {
            slot.item.GetComponent<BoxCollider>().enabled = false;
        }
    }

}
