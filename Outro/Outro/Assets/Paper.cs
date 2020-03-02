using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DG.Tweening;

public class Paper : MonoBehaviour
{
    SpriteRenderer _sprite;
    Vector3 _initPos;

    private void Awake() {
        _sprite = GetComponent<SpriteRenderer>();
        _initPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            FlyAway();
        }
    }

    public void SetImage(string folder, string filename) {
        _sprite.sprite = LoadSprite(folder + filename);
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
