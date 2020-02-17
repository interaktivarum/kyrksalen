using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickableItem : MonoBehaviour {

    public bool _packable;

    Vector3 _pickupPos;
    bool _draggable = true;
    //bool _correctDrop;
    DropAreaItems _dropArea;
    bool _hoveringDropArea;

    ViewPack _view;

    // Start is called before the first frame update
    void Start() {
        GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
        _dropArea = FindObjectOfType<DropAreaItems>();
        _view = GetComponentInParent<ViewPack>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {
        SetDropAreaState(true);
        AreaPickupTest();
        _hoveringDropArea = false;
        GetComponent<SpriteRenderer>().sortingOrder += 1;

        if (_draggable) {
            _pickupPos = transform.localPosition;
            GetComponent<SpriteRenderer>().color = Color.white;
            UpdatePosition();
        }
    }

    private void OnMouseUp() {
        SetDropAreaState(false);
        AreaDropTest();
        GetComponent<SpriteRenderer>().sortingOrder -= 1;
        GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
    }

    private void OnMouseDrag() {
        if (_draggable) {
            UpdatePosition();
        }
    }

    void UpdatePosition() {
        RaycastHit[] hits;

        Vector3 input = Input.mousePosition;
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(input.x, input.y, input.z - Camera.main.transform.position.z));

        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(input));

        bool hoverDropAreaPrev = _hoveringDropArea;
        _hoveringDropArea = false;
        foreach (RaycastHit hit in hits) {
            _hoveringDropArea = hit.transform.gameObject.GetComponent<DropAreaItems>();
            if (_hoveringDropArea) {
                pos += (_dropArea.transform.position - transform.position) / 8;
                break;
            }
        }
        EnterDropAreaTest(hoverDropAreaPrev);

        transform.DOLocalMove(pos, 0.25f);

    }

    public void EnterDropAreaTest(bool prev) {

        if (_hoveringDropArea && !prev) { //if enter drop area
            Color c = _dropArea.HasFreeSlot() ? Color.green : Color.red;
            GetComponent<SpriteRenderer>().color = c;
        }
        else if (prev && !_hoveringDropArea) { //if leave drop area
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void SetDropAreaState(bool state) {
        _dropArea.GetComponent<BoxCollider>().enabled = state;
    }

    void AreaDropTest() {
        if (_hoveringDropArea) {
            ItemSlot slot = _dropArea.AddItem(this);
            if (slot != null) {
                _view.SendStringToServer("ItemPacked:" + name);
                MoveToSlot(slot);
            }
            else {
                _view.SendStringToServer("ItemRejected:" + name);
                MoveToPickupPos();
            }
        }
    }

    void AreaPickupTest() {
        _dropArea.RemoveItem(this);
    }

    void MoveToSlot(ItemSlot slot) {
        transform.DOLocalMove(slot.position, 1);
    }

    void MoveToPickupPos() {
        transform.DOLocalMove(_pickupPos, 1);
    }

}
