using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewProfiles : MonoBehaviour
{
    public string serverName;
    public Profile[] profiles;
    public Transform objTransforms;
    public ViewMenu viewMenu;
    //public Profile selectedProfile;

    TCPMessageHandler _mh;

    private void Awake() {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start() {
        _mh = GameObject.FindObjectOfType<TCPMessageHandler>();
        _mh.AddCallback("UnloadView:"+name, UnloadView);
    }

    private void OnEnable() {
        LoadView();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ProfileHit(Profile pHit) {
        foreach (Profile p in profiles) {
            if (p == pHit) {
                p.Choose();
                viewMenu.GetComponent<ViewMenu>().SetCourses(p.courses);
                _mh.SendStringToServer("ChildSelected:" + p.name);
            }
            else {
                p.Disappear();
            }
        }
    }

    void LoadView() {
        foreach (Profile p in profiles) {
            p.gameObject.SetActive(true);
            p.Appear();
        }
    }

    public void UnloadView(string args) {
        StartCoroutine(UnloadProfiles());
    }

    IEnumerator UnloadProfiles() {
        YieldInstruction yi = null;
        foreach (Profile p in profiles) {
            if (p.gameObject.activeSelf) {
                yi = p.Disappear();
            }
        }
        yield return yi; //Note! Only the last animation counts
        gameObject.GetComponentInParent<Views>().NextView();

    }
}
