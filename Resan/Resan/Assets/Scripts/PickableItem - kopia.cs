using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickableItemOld : MonoBehaviour {

    public bool _packable;

    Vector3 _pickupPos;
    bool _draggable = true;
    DropAreaItem _daDrag = null;
    DropAreaItem _daDrop = null;
    bool _correctDrop;
    DropAreaItem[] _dropAreas;

    // Start is called before the first frame update
    void Start() {
        GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
        _dropAreas = FindObjectsOfType<DropAreaItem>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {
        SetDropAreasStates(true);
        AreaPickupTest();
        GetComponent<SpriteRenderer>().sortingOrder += 1;

        if (_draggable) {
            _pickupPos = transform.localPosition;
            GetComponent<SpriteRenderer>().color = Color.white;
            UpdatePosition(true);
        }
        
    }

    private void OnMouseUp() {
        SetDropAreasStates(false);
        AreaDropTest();
        if (!_daDrop) {
            transform.DOLocalMove(_pickupPos, 0.5f);
        }
        GetComponent<SpriteRenderer>().sortingOrder -= 1;
        GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
    }

    private void OnMouseDrag() {
        if (_draggable) {
            UpdatePosition(true);
        }
    }

    void UpdatePosition(bool smooth) {
        RaycastHit[] hits;

        Vector3 input = Input.mousePosition;

        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(input.x, input.y, input.z - Camera.main.transform.position.z));
        if (smooth) {
            transform.DOLocalMove(pos, 0.25f);
        }
        else {
            transform.localPosition = pos;
        }

        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(input));

        DropAreaItem dropAreaPrev = _daDrag;
        _daDrag = null;
        foreach (RaycastHit hit in hits) {
            _daDrag = hit.transform.gameObject.GetComponent<DropAreaItem>();
            if (_daDrag) {
                DragTest();
                break;
            }
        }
        EnterDropAreaTest(dropAreaPrev);
    }

    public void DragTest() {
        if (!_daDrag.HasItem()) {
            transform.DOLocalMove(_daDrag.transform.position, 0.25f);
        }
    }

    public void EnterDropAreaTest(DropAreaItem prev) {

        if (_daDrag && _daDrag != prev) { //if enter drop area
            bool droppable = !_daDrag.HasItem();
            Color c = droppable ? Color.green : Color.red;
            GetComponent<SpriteRenderer>().DOColor(c, 0.25f);
        }
        else if (prev && !_daDrag) { //if leave drop area
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void SetDropAreasStates(bool state) {
        foreach(DropAreaItem dropArea in _dropAreas) {
            dropArea.GetComponent<BoxCollider>().enabled = state;
        }
    }

    void AreaDropTest() {
        if (_daDrag) {
            if (!_daDrag.HasItem()) {
                _daDrop = _daDrag;
            }
            if (_daDrop) {
                transform.DOLocalMove(_daDrop.transform.position, 0.5f);
                //_daDrop.SetItem(this);
            }
            
        }
    }

    void AreaPickupTest() {
        if (_daDrop) {
            if (_daDrop.HasItem()) {
                _daDrop.SetItem(null);
            }
        }
    }

}
