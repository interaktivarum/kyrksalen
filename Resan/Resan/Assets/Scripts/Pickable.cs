using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pickable : MonoBehaviour
{
    float _z;
    Vector3 _mouseDist;

    // Start is called before the first frame update
    void Start()
    {
        _z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() {
        //transform.DOLocalMoveZ(0,0.25f);
        _mouseDist = Camera.main.WorldToScreenPoint(transform.localPosition) - Input.mousePosition;
        UpdatePosition();
    }

    private void OnMouseUp() {
        //transform.DOLocalMoveZ(_z, 0.25f);
        transform.DOLocalMoveY(0, 0.2f);
    }

    private void OnMouseDrag() {
        UpdatePosition();

    }

    void UpdatePosition() {
        RaycastHit[] hits;

        Vector3 input = Input.mousePosition + _mouseDist;
        input.x = Mathf.Max(input.x, 0);
        input.x = Mathf.Min(input.x, Camera.main.pixelWidth);

        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(input));

        float z = _z;
        Vector3 pushMargin = new Vector3();
        foreach (RaycastHit hit in hits) {

            if (hit.transform.gameObject.GetComponent<DropArea>()) {
                z = 0;
                break;
            }
            if (hit.transform.gameObject.GetComponent<RoomBound>()) {
                z = hit.point.z;
                pushMargin = hit.transform.gameObject.GetComponent<RoomBound>().margin;
            }
        }

        Vector3 posNew = Camera.main.ScreenToWorldPoint(new Vector3(input.x, input.y, z - Camera.main.transform.position.z));
        posNew += pushMargin;
        transform.DOLocalMove(posNew, 0.1f);

    }
}
