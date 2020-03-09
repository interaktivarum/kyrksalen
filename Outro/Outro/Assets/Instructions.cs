using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Instructions : MonoBehaviour
{

    SpriteRenderer[] _sprites;

    // Start is called before the first frame update
    void Start()
    {
        _sprites = GetComponentsInChildren<SpriteRenderer>();
        HideSprites();
        ShowScan();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Tween SetSpriteOpacity(int id, float opacity) {
        return _sprites[id].DOColor(new Color(1, 1, 1, 1), opacity);
    }

    public void HideSprites() {
        foreach (SpriteRenderer sprite in _sprites) {
            sprite.DOColor(new Color(1, 1, 1, 0), 1);
        }
    }

    public void ShowTryAgain() {
        Sequence seq = DOTween.Sequence();
        seq.Append(SetSpriteOpacity(1, 1));
        seq.AppendInterval(2);
        seq.Append(SetSpriteOpacity(1, 0));
        seq.Append(SetSpriteOpacity(0, 1));
    }

    public void ShowScan() {
        SetSpriteOpacity(0, 1);
    }

}
