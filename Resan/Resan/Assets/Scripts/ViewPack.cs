using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPack : ViewBase
{
    DropAreaItems[] _dropAreas;
    PickableItem[] _items;
    public int _dropAreaId;
    DropAreaItems _dropArea;
    int _sortingOrder = 0;
    AudioSource _audioSource;
    public int _maxItems;

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void SetReferences() {
        base.SetReferences();
        _audioSource = GetComponent<AudioSource>();
        _dropAreas = GetComponentsInChildren<DropAreaItems>(true);
        _items = GetComponentsInChildren<PickableItem>(true);
        foreach (DropAreaItems area in _dropAreas) {
            area.PopulateSlots();
        }
    }

    public override void LoadView() {
        base.LoadView();
        _sortingOrder = 0;
        _dropAreaId = -1;
        foreach(DropAreaItems area in _dropAreas) {
            area.gameObject.SetActive(false);
        }
        ActivateNextLayer();
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public void OnItemDropped() {
        /*int nPacked = 0;
        foreach(PickableItem item in _items) {
            if (item._packed) {
                nPacked++;
            }
        }
        Debug.Log(nPacked);*/
    }

    public void OnItemPacked() {
        /* if (!GetActiveDropArea().HasFreeSlot()) {
             GetActiveDropArea().gameObject.SetActive(false);
             GetActiveDropArea().LockItems();
             if (_dropAreaId < _dropAreas.Length - 1) {
                 ActivateNextLayer();
             }
             else {
                 SendStringToServer("AllItemsPacked:1");
                 InitUnloadView();
             }
         }*/
        int nPacked = 0;
        foreach (PickableItem item in _items) {
            if (item._packed) {
                nPacked++;
            }
        }
        if(nPacked >= _maxItems) {
            SendStringToServer("AllItemsPacked:1");
            InitUnloadView();
            LockItems();
        }
    }

    void ActivateNextLayer() {
        _dropAreaId++;
        GetActiveDropArea().gameObject.SetActive(true);
        SendStringToServer("ActivateLayer:"+ _dropAreaId);        
    }

    public DropAreaItems GetActiveDropArea() {
        return _dropAreas[_dropAreaId];
    }

    public override YieldInstruction DoUnloadView() {
        return new WaitForSeconds(2);
    }

    public int NextSortingOrder() {
        return ++_sortingOrder;
    }

    public void PlaySound(AudioClip clip) {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    void LockItems() {
        foreach (PickableItem item in _items) {
            item.GetComponent<BoxCollider>().enabled = false;
        }
    }

}
