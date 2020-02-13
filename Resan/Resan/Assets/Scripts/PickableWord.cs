using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickableWord : MonoBehaviour
{
    Vector3 _pickupPos;
    Vector3 _pickupRot;
    bool _draggable = true;
    DropAreaWord _dropAreaHover;
    //bool _correctDrop;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   
    }

    private void OnMouseDown() {
        if (_draggable) {
            _pickupPos = transform.localPosition;
            _pickupRot = transform.localRotation.eulerAngles;
            GetComponent<SpriteRenderer>().color = Color.white;
            UpdatePosition();
        }
    }

    private void OnMouseUp() {
        GetComponent<SpriteRenderer>().color = Color.white;
        AreaDropTest();
    }

    private void OnMouseDrag() {
        if (_draggable) {
            UpdatePosition();
        }
    }

    void UpdatePosition() {
        RaycastHit[] hits;

        Vector3 input = Input.mousePosition;

        Vector3 posNew = Camera.main.ScreenToWorldPoint(new Vector3(input.x, input.y, input.z - Camera.main.transform.position.z));
        transform.DOLocalMove(posNew, 0.1f);

        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(input));

        DropAreaWord dropAreaPrev = _dropAreaHover;
        _dropAreaHover = null;
        foreach (RaycastHit hit in hits) {
            _dropAreaHover = hit.transform.gameObject.GetComponent<DropAreaWord>();
            if (_dropAreaHover) {
                DragTest();
                break;
            }
        }
        EnterDropAreaTest(dropAreaPrev);
    }

    public void DragTest() {
        transform.DOLocalMove(_dropAreaHover.transform.localPosition, 0.1f);
    }

    public void EnterDropAreaTest(DropAreaWord prev) {
        if (_dropAreaHover && _dropAreaHover != prev) { //if enter drop area
            transform.DOLocalRotate(_dropAreaHover.transform.localEulerAngles, 0.25f);
            Color c = DroppableArea(_dropAreaHover) ? Color.green : Color.red;
            GetComponent<SpriteRenderer>().DOColor(c, 0.25f);
        }
        else if (prev && !_dropAreaHover) { //if leave drop area
            transform.DOLocalRotate(_pickupRot, 0.25f);
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void AreaDropTest() {
        if (_dropAreaHover) {
            if (DroppableArea(_dropAreaHover)) {
                _draggable = false;
                _dropAreaHover.SetSolved(true);
            }
            else {
                transform.DOLocalMove(_pickupPos, 0.5f);
                transform.DOLocalRotate(_pickupRot, 0.25f);
            }
        }
    }

    void AreaPickupTest() {
    }

    bool DroppableArea(DropAreaWord dropArea) {
        return dropArea.GetComponent<SpriteRenderer>().sprite == GetComponent<SpriteRenderer>().sprite;
    }

}
