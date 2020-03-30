using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPack : ViewBase
{
    DropAreaItems[] _dropAreas;
    public int _dropAreaId;
    DropAreaItems _dropArea;
    int _sortingOrder = 0;
    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void SetReferences() {
        base.SetReferences();
        _audioSource = GetComponent<AudioSource>();
        _dropAreas = GetComponentsInChildren<DropAreaItems>(true);
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

    public void OnItemPacked() {
        if (!GetActiveDropArea().HasFreeSlot()) {
            GetActiveDropArea().gameObject.SetActive(false);
            GetActiveDropArea().LockItems();
            if (_dropAreaId < _dropAreas.Length - 1) {
                ActivateNextLayer();
            }
            else {
                SendStringToServer("AllItemsPacked:1");
                InitUnloadView();
            }
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

}
