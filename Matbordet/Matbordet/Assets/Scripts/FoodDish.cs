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

    private void Awake() {
    }

    // Start is called before the first frame update
    void Start()
    {
        _initScale = transform.localScale;
        //SetSprite();
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

    /*void SetSprite () {

        _sprite = GetComponentInChildren<SpriteRenderer>().sprite;

        Bounds br = gameObject.GetComponentInChildren<SpriteRenderer>().bounds;
        //Debug.Log("Renderer bounds: " + br);

        Vector3 blb = new Vector3(br.center.x - br.extents.x, br.center.y - br.extents.y, 0);
        _bl = Camera.main.WorldToScreenPoint(blb);

        Vector3 urb = new Vector3(br.center.x + br.extents.x, br.center.y + br.extents.y, 0);
        _ur = Camera.main.WorldToScreenPoint(urb);

        _pixelSize = _ur - _bl;
        //Debug.Log("Pixel size: " + _pixelSize);

        _scale = _pixelSize / _sprite.rect.size;
        //Debug.Log("Scale: " + _scale);

        Debug.Log(dishName);
        Debug.Log(_bl);
        Debug.Log(_ur);
        Debug.Log(_pixelSize);
        Debug.Log(_sprite.rect.size);
    }*/

    //public bool HitTest() {
        /*
        //Debug.Log("Click: " + dishName);      

        //Debug.Log("Screen click: " + Input.mousePosition);

        Vector2 clickObject = new Vector2(Input.mousePosition.x - _ul.x, Input.mousePosition.y - _br.y);
        //Debug.Log("Object click: " + clickObject);

        Vector2 clickTexture = clickObject / _scale;
        //Debug.Log("Texture click: " + clickTexture);

        Color color = _sprite.texture.GetPixel((int)clickTexture.x, (int)clickTexture.y);
        //Debug.Log(color);

        return color.a != 0;
        */

        /*
        Vector2 mouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        return mouse.x > _bl.x && mouse.y > _bl.y && mouse.x < _ur.x && mouse.y < _ur.y;
        */

    //}

    public void Appear() {
        transform.localRotation = new Quaternion(0, 180, 180, 0);
        transform.localScale = new Vector3();
        transform.DORotate(new Vector3(), 1f);
        Tween myTween = transform.DOScale(new Vector3(1,1,1), 1f);
    }

    public YieldInstruction Disappear() {
        transform.DORotate(new Vector3(0, 180, 180), 1f);
        Tween myTween = transform.DOScale(new Vector3(), 1f);
        //myTween.OnComplete(() => Destroy(gameObject));
        //myTween.WaitForCompletion()
        myTween.OnComplete(() => gameObject.SetActive(false));
        return myTween.WaitForCompletion();
    }

    public void Select() {
        transform.DORotate(new Vector3(0, 0, 10), 1f);
        Tween myTween = transform.DOScale(transform.localScale * 1.25f, 1f);
    }

    public void Deselect() {
        transform.DORotate(new Vector3(0, 0, 0), 1f);
        Tween myTween = transform.DOScale(new Vector3(1, 1, 1), 1f);
    }

}