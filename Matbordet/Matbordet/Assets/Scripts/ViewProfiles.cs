using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewProfiles : MonoBehaviour
{
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
    }

    private void OnEnable() {
        LoadProfiles();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Transform hitTrans = null;
            foreach (Transform d in objTransforms) {
                Profile pScript = d.GetComponent<Profile>();
                if (pScript.HitTest()) {
                    hitTrans = d;
                }
            }
            if (hitTrans) {
                foreach (Transform p in objTransforms) {
                    Profile pScript = p.GetComponent<Profile>();
                    if (hitTrans == p) {
                        pScript.Choose();
                        viewMenu.GetComponent<ViewMenu>().SetCourses(pScript.courses);

                        //Send server message
                        /*_mh.SendMessageToServer(new JsonMessage("ChildSelected") {
                            args = "{\"id\": \"" + pScript.serverName + "\"}"
                        });*/
                        _mh.SendStringToServer("ChildSelected" + pScript.serverName);
                    }
                    else {
                        pScript.Disappear();
                    }
                }
                
            }
        }
        if (objTransforms.childCount == 0) {
            gameObject.GetComponentInParent<Views>().NextView();
        }
    }

    void LoadProfiles() {

        foreach (Profile p in profiles) {
            GameObject profile = Instantiate(p.gameObject, objTransforms);
            profile.SetActive(true);
            //p.Appear();
        }
    }
}
