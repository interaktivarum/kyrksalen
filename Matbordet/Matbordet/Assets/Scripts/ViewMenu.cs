using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMenu : MonoBehaviour
{

    public FoodCourse[] courses;
    private int _courseId = 0;
    //public Transform objTransforms;

    TCPMessageHandler _mh;
    

    private void Awake() {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _mh = FindObjectOfType<TCPMessageHandler>();
        _mh.AddCallback("UnloadMenu", UnloadView);
    }

    private void OnEnable() {
        LoadMenu();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DishHit(FoodDish dish) {
        foreach (FoodCourse c in courses) {

            bool courseHasDish = false;
            foreach (FoodDish d in c.dishes) {
                if (d == dish) {
                    courseHasDish = true;
                }
            }

            if (courseHasDish) {
                //If hit, iterate to perform action 
                if (dish == c.correct) {
                    c.correctSelected = true;
                    Debug.Log("Correct");
                    foreach (FoodDish d in c.dishes) {

                        if (dish == d) {
                            d.Choose();
                            _courseId++;

                            //Send server message
                            /*_mh.SendMessageToServer(new JsonMessage("DishSelected") {
                                args = "{\"id\": \"" + d.serverName + "\"}"
                            });*/
                            _mh.SendStringToServer("DishSelected" + d.serverName);
                        }
                        else {
                            d.Disappear();
                        }

                    }
                    if (AllCorrectSelected()) {
                        _mh.SendStringToServer("AllDishesSelected");
                    }
                }
                else {
                    c.correctSelected = false;
                }
            }
        }
    }

    void LoadMenu() {
        foreach (FoodCourse c in courses) {
            foreach (FoodDish d in c.dishes) {
                d.gameObject.SetActive(true);
                d.Appear();
            }
        }
    }

    public void UnloadView(string args) {
        StartCoroutine(UnloadMenu());
    }

    IEnumerator UnloadMenu() {
        YieldInstruction yi = null;
        foreach (FoodCourse c in courses) {
            foreach (FoodDish d in c.dishes) {
                if (d.gameObject.activeSelf) {
                    yi = d.Disappear();
                }
            }
        }
        yield return yi; //Note! Only the last animation counts
        gameObject.GetComponentInParent<Views>().NextView();

    }

    /*void LoadCourse(int id) {
        foreach (FoodDish d in courses[id].dishes) {
            GameObject dish = Instantiate(d.gameObject, objTransforms);
            //FoodDish dScript = dish.GetComponent<FoodDish>();
            dish.SetActive(true);
            //d.Appear();
            //d.SetActive(false);
        }
    }*/

    public void SetCourses(FoodCourse[] c) {
        courses = c;
        _courseId = 0;
    }

    bool AllCorrectSelected() {
        bool allCorrectSelected = true;
        foreach (FoodCourse c in courses) {

            if (!c.correctSelected) {
                allCorrectSelected = false;
            }
        }
        return allCorrectSelected;
    }

}
