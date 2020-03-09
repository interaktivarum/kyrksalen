using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DG.Tweening;

public class Paper : MonoBehaviour
{
    SpriteRenderer _sprite;
    Vector3 _initPos;
    Material _material;

    private void Awake() {
        _sprite = GetComponent<SpriteRenderer>();
        _initPos = transform.position;
        _material = GetComponentInChildren<MeshRenderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0)) {
            FlyAway();
        }*/

        //_material.SetFloat("_Distort", Mathf.Cos(Time.time / 4));
    }

    public void Appear() {
        _material.SetFloat("_Distort", 1);
        Sequence seq = DOTween.Sequence();
        seq.Append(_material.DOFloat(0, "_Distort", 3));
        //seq.AppendInterval(2);
        //seq.Append(_material.DOFloat(0, "_Distort", 3));
    }

    public void Disappear() {
        Sequence seq = DOTween.Sequence();
        seq.Append(_material.DOFloat(1, "_Distort", 3));
    }

    public void SetImage(string folder, string filename) {

        Texture2D tex = LoadTexture(folder + filename);
        _material.mainTexture = tex;
        //_sprite.sprite = LoadSprite(folder + filename);
        //transform.localScale = new Vector3(_sprite.bounds.size.x / _sprite.bounds.size.y, 1, 1);

        //float ratio = _sprite.bounds.size.x / _sprite.bounds.size.y;
        //_material.SetFloat("_Ratio", _sprite.bounds.size.x / _sprite.bounds.size.y);
        _material.SetFloat("_Ratio", tex.width / tex.height);

        //_material.SetTextureScale("_MainTex", new Vector2(1 / ratio, 1));
        //_material.SetTextureOffset("_MainTex", new Vector2((1 - (1 / ratio)) / 2, 0));
    }

    Texture2D LoadTexture(string filePath) {
        Debug.Log(filePath);
        Texture2D tex = null;
        byte[] fileData;
        if (File.Exists(filePath)) {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    Sprite LoadSprite(string filePath) {
        Texture2D tex = null;
        byte[] fileData;
        if (File.Exists(filePath)) {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 100);
        return sprite;
    }

    void FlyAway() {

        GetComponent<Rigidbody>().AddForceAtPosition(
            new Vector3(-1, 1, 2)/5, new Vector3(-10,0, 0), ForceMode.Impulse
            );

        transform.DOLocalMoveY(10, 1).SetRelative(true).SetEase(Ease.OutCubic).OnComplete(
            () => {
                transform.DOMoveY(100, 1).SetRelative(true).SetEase(Ease.InCubic);
                transform.DOLocalRotate(new Vector3(0, 90, 0), 2).SetRelative(true);
            });

    }
}
