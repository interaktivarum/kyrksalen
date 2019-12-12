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

    /*void SetSprite() {

        _sprite = GetComponentInChildren<SpriteRenderer>().sprite;

        Bounds br = gameObject.GetComponentInChildren<SpriteRenderer>().bounds;

        Vector3 ulb = new Vector3(br.center.x - br.extents.x, br.center.y + br.extents.y, 0);
        _ul = Camera.main.WorldToScreenPoint(ulb);

        Vector3 brb = new Vector3(br.center.x + br.extents.x, br.center.y - br.extents.y, 0);
        _br = Camera.main.WorldToScreenPoint(brb);

        _pixelSize = new Vector2(_br.x - _ul.x, _ul.y - _br.y);

        _scale = _pixelSize / _sprite.rect.size;
    }*/

    /*public bool HitTest() {
        Vector2 clickObject = new Vector2(Input.mousePosition.x - _ul.x, Input.mousePosition.y - _br.y);
        Vector2 clickTexture = clickObject / _scale;
        Color color = _sprite.texture.GetPixel((int)clickTexture.x, (int)clickTexture.y);
        return color.a != 0;
    }*/

    public void Appear() {
        transform.localRotation = new Quaternion(0, 180, 180, 0);
        transform.localScale = new Vector3();
        transform.DORotate(new Vector3(), 1f);
        Tween myTween = transform.DOScale(new Vector3(1, 1, 1), 1f);
    }

    public YieldInstruction Disappear() {
        //Transform sprite = transform.Find("Sprite");
        transform.DORotate(new Vector3(0, 180, 180), 1f);
        Tween myTween = transform.DOScale(new Vector3(), 1f);
        myTween.OnComplete(() => gameObject.SetActive(false));
        return myTween.WaitForCompletion();
    }

    public YieldInstruction Choose() {
        transform.DORotate(new Vector3(0, 0, 10), 1f);
        Tween myTween = transform.DOScale(transform.localScale * 1.25f, 1f);
        //myTween.OnComplete(() => Disappear());
        return myTween.WaitForCompletion();
    }

}