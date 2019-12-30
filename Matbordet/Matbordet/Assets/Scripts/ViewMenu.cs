using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMenu : ViewBase {

    public string serverName;
    public FoodCourse[] courses;
    
    //private int _courseId = 0;
    //public Transform objTransforms;

    TCPMessageHandler _mh;
    

    private void Awake() {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _mh = FindObjectOfType<TCPMessageHandler>();
        _mh.AddCallback("UnloadView:"+name, UnloadView);
    }

    private void OnEnable() {
        LoadView();
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

                foreach (FoodDish d in c.dishes) {
                    if (dish == d) {
                        d.Select();        
                        _mh.SendStringToServer("DishSelected:" + d.name);
                        c.correctSelected = dish == c.correct;
                        //_courseId++;
                    }
                    else {
                        if (dish == c.correct) {
                            d.Disappear();
                        }
                        else {
                            d.Deselect();
                        }
                    }
                }
                if (AllCorrectSelected()) {
                    _mh.SendStringToServer("AllDishesSelected");
                }
            }
                
            
        }
    }

    void LoadView() {
        foreach (FoodCourse c in courses) {
            foreach (FoodDish d in c.dishes) {
                d.gameObject.SetActive(true);
                d.Appear();
            }
        }
    }

    public override void UnloadView(string args) {
        //base.UnloadView(args);
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
        //_courseId = 0;
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
