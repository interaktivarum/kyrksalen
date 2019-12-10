using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMenu : MonoBehaviour
{

    public FoodCourse[] courses;
    private int _courseId = 0;
    public Transform objTransforms;

    TCPMessageHandler _mh;

    private void Awake() {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _mh = GameObject.FindObjectOfType<TCPMessageHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Transform hitTrans = null;
            foreach (Transform d in objTransforms) {
                FoodDish dScript = d.GetComponent<FoodDish>();
                if (dScript.HitTest()) {
                    hitTrans = d;
                }
            }
            //Debug.Log(hitTrans);
            if (hitTrans) {
                foreach (Transform d in objTransforms) {
                    FoodDish dScript = d.GetComponent<FoodDish>();
                    if (hitTrans == d) {
                        dScript.Choose();
                        _courseId++;

                        //Send server message
                        _mh.SendMessageToServer(new JsonMessage("DishSelected") {
                            args = "{\"id\": \"" + dScript.serverName + "\"}"
                        });
                    }
                    else {
                        dScript.Disappear();
                    }
                }
            }
        }
        if(objTransforms.childCount == 0) {
            if(_courseId < courses.Length) {
                LoadCourse(_courseId);
            }
            else {
                gameObject.GetComponentInParent<Views>().Restart();
            } 
        }
    }

    void LoadCourse(int id) {
        foreach (FoodDish d in courses[id].dishes) {
            GameObject dish = Instantiate(d.gameObject, objTransforms);
            //FoodDish dScript = dish.GetComponent<FoodDish>();
            dish.SetActive(true);
            //d.Appear();
            //d.SetActive(false);
        }
    }

    public void SetCourses(FoodCourse[] c) {
        courses = c;
        _courseId = 0;
    }

}
