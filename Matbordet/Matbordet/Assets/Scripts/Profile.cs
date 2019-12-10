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
        SetSprite();
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    void SetSprite() {

        _sprite = GetComponentInChildren<SpriteRenderer>().sprite;

        Bounds br = gameObject.GetComponentInChildren<SpriteRenderer>().bounds;

        Vector3 ulb = new Vector3(br.center.x - br.extents.x, br.center.y + br.extents.y, 0);
        _ul = Camera.main.WorldToScreenPoint(ulb);

        Vector3 brb = new Vector3(br.center.x + br.extents.x, br.center.y - br.extents.y, 0);
        _br = Camera.main.WorldToScreenPoint(brb);

        _pixelSize = new Vector2(_br.x - _ul.x, _ul.y - _br.y);

        _scale = _pixelSize / _sprite.rect.size;
    }

    public bool HitTest() {
        Vector2 clickObject = new Vector2(Input.mousePosition.x - _ul.x, Input.mousePosition.y - _br.y);
        Vector2 clickTexture = clickObject / _scale;
        Color color = _sprite.texture.GetPixel((int)clickTexture.x, (int)clickTexture.y);
        return color.a != 0;
    }

    public void Appear() {
        transform.Rotate(0, 180, 180);
        Vector3 initScale = transform.localScale;
        transform.localScale = new Vector3();
        transform.DORotate(new Vector3(), 1f);
        Tween myTween = transform.DOScale(initScale, 1f);
    }

    public void Disappear() {
        Transform sprite = transform.Find("Sprite");
        sprite.DORotate(new Vector3(0, 180, 180), 1f);
        Tween myTween = sprite.DOScale(new Vector3(), 1f);
        myTween.OnComplete(() => Destroy(gameObject));
    }

    public void Choose() {
        transform.DORotate(new Vector3(0, 0, 10), 1f);
        Tween myTween = transform.DOScale(transform.localScale * 1.25f, 1f);
        myTween.OnComplete(Disappear);
    }

}