using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickableItem : MonoBehaviour {

    Vector3 _initPos;
    public bool _packable;
    public Vector2 dimensions;
    public bool _packed;

    //Sounds
    public AudioClip[] sounds;
    int _soundId;

    Vector3 _pickupPos;
    bool _draggable = true;
    //bool _correctDrop;
    //DropAreaItems _dropArea;
    DropAreaReject[] _rejectAreas;
    bool _hoveringDropArea;

    ViewPack _view;

    private void Awake() {
        _initPos = transform.localPosition;
    }

    // Start is called before the first frame update
    void Start() {
        GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
        //_dropArea = FindObjectOfType<DropAreaItems>();
        _view = GetComponentInParent<ViewPack>();
        _rejectAreas = _view.GetComponentsInChildren<DropAreaReject>(true);
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnEnable() {
        transform.localPosition = _initPos;
        _draggable = true;
        SetBoxColliderBounds();
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        GetComponent<BoxCollider>().enabled = true;
        // GetComponent<BoxCollider>().center = new Vector3();
        _packed = false;
    }

    void SetBoxColliderBounds () {
        gameObject.GetComponent<BoxCollider>().size = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
    }

    private void OnMouseDown() {
        SetDropAreaState(true);
        PlaceOnTop();
        _packed = false;
        AreaPickupTest();
        _hoveringDropArea = false;
        //GetComponent<SpriteRenderer>().sortingOrder += 1;

        if (_draggable) {
            _pickupPos = transform.localPosition;
            GetComponent<SpriteRenderer>().color = Color.white;
            UpdatePosition();
        }
    }

    private void OnMouseUp() {
        if (!AreaDropTest()) {
            RejectTest();
        }
        _view.OnItemDropped();
        //GetComponent<SpriteRenderer>().sortingOrder -= 1;
        GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
        SetDropAreaState(false);
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
                pos += (GetDropArea().transform.position - transform.position) / 8;
                break;
            }
        }
        EnterDropAreaTest(hoverDropAreaPrev);

        pos.z = transform.position.z;
        transform.DOLocalMove(pos, 0.25f);

    }

    public void EnterDropAreaTest(bool prev) {

        if (_hoveringDropArea && !prev) { //if enter drop area
            //Color c = _dropArea.HasFreeSlot() ? Color.green : Color.red;
            Color c = _packable ? Color.green : Color.red;
            GetComponent<SpriteRenderer>().color = c;
        }
        else if (prev && !_hoveringDropArea) { //if leave drop area
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void SetDropAreaState(bool state) {
        GetDropArea().GetComponent<BoxCollider>().enabled = state;
        foreach(DropAreaReject area in _rejectAreas) {
            area.GetComponent<BoxCollider>().enabled = state;
        }
    }

    bool AreaDropTest() {
        if (_hoveringDropArea) {
            PlaySound();
            bool packing = false;
            if (_packable) {
                List<ItemSlot> slots = GetDropArea().AddItem(this);
                if (slots != null && slots.Count > 0) {
                    _view.SendStringToServer("ItemPacked:" + name);
                    MoveToSlotsCenter(slots);
                    packing = true;
                    _packed = true;
                    _view.OnItemPacked();
                }
            }
            if (!packing) {
                MoveToPickupPos();
            }
            return true;
        }
        return false;
    }

    bool RejectTest() {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
        foreach (RaycastHit hit in hits) {
            if (hit.transform.gameObject.GetComponent<DropAreaReject>()) {
                MoveToInitPos();
                return true;
            }
        }
        return false;
    }

    void AreaPickupTest() {
        GetDropArea().RemoveItem(this);
    }

    void MoveToSlotsCenter(List<ItemSlot> slots) {
        Vector3 center = new Vector3();
        foreach(ItemSlot slot in slots) {
            center += slot.position;
        }
        center /= slots.Count;
        center.z = -GetComponent<SpriteRenderer>().sortingOrder / 10000f;
        transform.DOLocalMove(center, 1);
    }

    void MoveToPickupPos() {
        transform.DOLocalMove(_pickupPos, 1);
    }

    void MoveToInitPos() {
        transform.DOLocalMove(_initPos, 1);
    }

    void PlaceOnTop() {
        GetComponent<SpriteRenderer>().sortingOrder = _view.NextSortingOrder();
    }

    DropAreaItems GetDropArea() {
        return _view.GetActiveDropArea();
    }

    void PlaySound() {
        if(sounds.Length > 0) {
            _view.PlaySound(sounds[_soundId]);
            _soundId = (_soundId + 1) % sounds.Length;
        }
    }

}
