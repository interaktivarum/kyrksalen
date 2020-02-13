using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoodDish : MonoBehaviour
{

    public string dishName;
    public string serverName;
    private Sprite _sprite;
    private Vector2 _bl;
    private Vector2 _ur;
    private Vector2 _pixelSize;
    private Vector2 _scale;
    private Vector2 _initScale;
    private Vector3 _initPos;

    private void Awake() {
        _initPos = transform.localPosition;
        _initScale = transform.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable() {
        //Appear();
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown() {
        GetComponentInParent<ViewMenu>().DishHit(this);
    }

    public void Appear() {
        transform.localPosition = _initPos;
        transform.localRotation = new Quaternion(0, 180, 180, 0);
        transform.localScale = new Vector3();
        transform.DORotate(new Vector3(), 1f);
        Tween myTween = transform.DOScale(new Vector3(1,1,1), 1f);
    }

    public YieldInstruction Disappear() {
        transform.DORotate(new Vector3(0, 180, 180), 1f);
        Tween myTween = transform.DOScale(new Vector3(), 1f);
        myTween.OnComplete(() => gameObject.SetActive(false));
        return myTween.WaitForCompletion();
    }

    public void Select() {
        Sequence seq = DOTween.Sequence();
        //transform.DORotate(new Vector3(0, 0, 10), 1f);
        seq.Append(transform.DOScale(0.9f, 0.2f));
        seq.Append(transform.DOScale(1, 0.2f));
    }

    public void SelectCorrect() {
        Sequence seq = DOTween.Sequence();
        seq.SetDelay(1);
        seq.Append(transform.DOLocalMoveX(0, 1));
    }

    public void Deselect() {
        transform.DORotate(new Vector3(0, 0, 0), 1f);
        Tween myTween = transform.DOScale(new Vector3(1, 1, 1), 1f);
    }

}