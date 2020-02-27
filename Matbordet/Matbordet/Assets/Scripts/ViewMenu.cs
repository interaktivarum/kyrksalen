using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ViewMenu : ViewBase {

    public string serverName;
    public FoodCourse[] courses;
    bool _lock;
    Image _bgText;
    //private int _courseId = 0;
    //public Transform objTransforms;

    TCPMessageHandler _mh;
    

    private void Awake() {
        gameObject.SetActive(false);
        _bgText = GetComponentInChildren<Canvas>().GetComponentsInChildren<Image>()[1];
    }

    // Start is called before the first frame update
    void Start()
    {
        _mh = FindObjectOfType<TCPMessageHandler>();
        _mh.AddCallback("DishMovieFinished", Unlock);
    }

    private void OnEnable() {
        _lock = false;
        HideBackground();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SetBackground(int id) {
        Transform t = GetComponentInChildren<Canvas>().transform;
        for (int i = 0; i < t.childCount; i++) {
            Image image = t.GetChild(i).GetComponent<Image>();
            image.gameObject.SetActive(i == id);
        }
    }

    public void DishHit(FoodDish dish) {
        if (!IsLocked()) {
            int iC = 0;
            foreach (FoodCourse c in courses) {

                bool courseHasDish = false;
                foreach (FoodDish d in c.dishes) {
                    if (d == dish) {
                        courseHasDish = true;
                    }
                }

                if (courseHasDish) {

                    int iD = 0;
                    foreach (FoodDish d in c.dishes) {
                        if (dish == d) {
                            d.Select();
                            if (dish == c.correct) {
                                c.correctSelected = true;
                                if (!AllCorrectSelected()) {
                                    dish.SelectCorrect();
                                } 
                            }
                            /*else {
                                //d.Select();
                            }*/
                            if (_mh.SendStringToServer("Dish" + iC + "Selected:" + iD)) {
                                if (!AllCorrectSelected()) {
                                    Lock();
                                }
                            }
                        }
                        else {
                            if (dish == c.correct) {
                                d.Disappear();
                            }
                            /*else {
                                d.Deselect();
                            }*/
                        }
                        iD++;
                    }
                    if (AllCorrectSelected()) {
                        Present();
                        _mh.SendStringToServer("AllDishesSelected:1");
                        views.BlockScreensaver();
                    }
                }

                iC++;
            }
        }
    }

    public override void LoadView() {
        base.LoadView();
        Unlock("");
        foreach (FoodCourse c in courses) {
            c.correctSelected = false;
            foreach (FoodDish d in c.dishes) {
                d.gameObject.SetActive(true);
                d.Appear();
            }
        }
    }

    /*public override void UnloadView() {
        Debug.Log("Unload Profiles view");
        //base.UnloadView(args);
        StartCoroutine(UnloadMenu());
    }*/

    /*public override YieldInstruction DoUnloadView() {
        YieldInstruction yi = null;
        foreach (FoodCourse c in courses) {
            foreach (FoodDish d in c.dishes) {
                if (d.gameObject.activeSelf) {
                    yi = d.Disappear();
                }
            }
        }
        return yi;
    }*/

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

    void Present() {
        _lock = true;
        foreach (FoodCourse c in courses) {
            c.correct.Present();
        }
        FadeBackground(1);// SetBackground(1);
        views.Dim(2, 8);
    }

    void HideBackground() {
        _bgText.color = new Color(1, 1, 1, 0);
    }

    void FadeBackground(int a = 1) {
        Sequence seq = DOTween.Sequence();
        seq.SetDelay(2);
        seq.Append(_bgText.DOColor(new Color(1, 1, 1, a), 1));
    }

    void Lock() {
        views.Dim(2,2);
        _lock = true;
    }

    void Unlock(string args) {
        if (!AllCorrectSelected()) {
            views.ResetFade();
            _lock = false;
        }
    }

    public bool IsLocked() {
        return _lock;
    }

}
