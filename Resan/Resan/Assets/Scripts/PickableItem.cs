using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickableItem : MonoBehaviour {

    public bool _packable;

    Vector3 _pickupPos;
    bool _draggable = true;
    bool _correctDrop;
    DropAreaItems _dropArea;
    bool _hoverDropArea;

    // Start is called before the first frame update
    void Start() {
        GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
        _dropArea = FindObjectOfType<DropAreaItems>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {
        SetDropAreaState(true);
        AreaPickupTest();
        _hoverDropArea = false;
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
        /*if (!_daDrop) {
            transform.DOLocalMove(_pickupPos, 0.5f);
        }*/
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

        bool hoverDropAreaPrev = _hoverDropArea;
        _hoverDropArea = false;
        foreach (RaycastHit hit in hits) {
            _hoverDropArea = hit.transform.gameObject.GetComponent<DropAreaItems>();
            if (_hoverDropArea) {
                pos += (_dropArea.transform.position - transform.position) / 8;
                break;
            }
        }
        EnterDropAreaTest(hoverDropAreaPrev);

        transform.DOLocalMove(pos, 0.25f);

    }

    public void EnterDropAreaTest(bool prev) {

        if (_hoverDropArea && !prev) { //if enter drop area
            Color c = _dropArea.HasFreeSlot() ? Color.green : Color.red;
            GetComponent<SpriteRenderer>().DOColor(c, 0.25f);
        }
        else if (prev && !_hoverDropArea) { //if leave drop area
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void SetDropAreaState(bool state) {
        _dropArea.GetComponent<BoxCollider>().enabled = state;
    }

    void AreaDropTest() {
        if (_hoverDropArea) {
            ItemSlot slot = _dropArea.AddItem(this);
            if (slot != null) {
                MoveToSlot(slot);
            }
            else {
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
