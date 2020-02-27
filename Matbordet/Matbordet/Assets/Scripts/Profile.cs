using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Profile : MonoBehaviour {

    public string profileName;
    public string serverName;
    public FoodCourse[] courses;
    private Sprite _sprite;
    private Vector2 _ul;
    private Vector2 _br;
    private Vector2 _pixelSize;
    private Vector2 _scale;

    private void Awake() {
        //SetSprite();
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnMouseDown() {
        GetComponentInParent<ViewProfiles>().ProfileHit(this);
    }

    public void Appear() {
        transform.localRotation = Quaternion.Euler(0, 20, 30);
        transform.localScale = new Vector3();
        transform.DORotate(new Vector3(), 1f);
        Tween myTween = transform.DOScale(new Vector3(1, 1, 1), 1f);
    }

    public YieldInstruction Disappear() {
        //Transform sprite = transform.Find("Sprite");
        transform.DORotate(new Vector3(0, 20, 30), 1f);
        Tween myTween = transform.DOScale(new Vector3(), 1f);
        myTween.OnComplete(() => gameObject.SetActive(false));
        return myTween.WaitForCompletion();
    }

    public YieldInstruction Choose() {
        transform.DORotate(new Vector3(0, 0, 5), 1f);
        Tween myTween = transform.DOScale(transform.localScale * 1.1f, 1f);
        //myTween.OnComplete(() => Disappear());
        return myTween.WaitForCompletion();
    }

}