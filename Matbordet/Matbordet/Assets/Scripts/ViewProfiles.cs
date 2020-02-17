using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewProfiles : ViewBase
{
    public string serverName;
    public Profile[] profiles;
    public Transform objTransforms;
    public ViewMenu viewMenu;
    public bool _selected;

    TCPMessageHandler _mh;

    private void Awake() {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start() {
        _mh = GameObject.FindObjectOfType<TCPMessageHandler>();
        _mh.AddCallback("UnloadView", UnloadView);
    }

    private void OnEnable() {
        LoadView();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ProfileHit(Profile pHit) {
        if (!_selected) {
            int iP = 0;
            foreach (Profile p in profiles) {
                if (p == pHit) {
                    _selected = true;
                    p.Choose();
                    views.Dim();
                    viewMenu.GetComponent<ViewMenu>().SetCourses(p.courses);
                    _mh.SendStringToServer("ChildSelected:" + iP);
                }
                else {
                    p.Disappear();
                }
                iP++;
            }
        }
    }

    void LoadView() {
        _selected = false;
        foreach (Profile p in profiles) {
            p.gameObject.SetActive(true);
            p.Appear();
        }
    }

    /*public override void UnloadView(string args) {
        base.UnloadView(args);
        //StartCoroutine(UnloadProfiles());
    }*/

    IEnumerator UnloadProfiles() {
        YieldInstruction yi = null;
        foreach (Profile p in profiles) {
            if (p.gameObject.activeSelf) {
                yi = p.Disappear();
            }
        }
        yield return yi; //Note! Only the last animation counts
        views.NextView();

    }
}
