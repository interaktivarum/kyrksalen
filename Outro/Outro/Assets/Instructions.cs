using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Instructions : MonoBehaviour
{

    SpriteRenderer[] _sprites;
    public SpriteRenderer spriteDefault;
    public SpriteRenderer spriteScan;
    public SpriteRenderer spriteSaved;
    public SpriteRenderer spriteBring;
    public SpriteRenderer spriteBox;
    public SpriteRenderer spriteError;
    public SpriteRenderer spriteRetry;
    Sequence _seq;

    ViewImage _view;

    // Start is called before the first frame update
    void Start()
    {
        _sprites = GetComponentsInChildren<SpriteRenderer>();
        HideSprites();
        ShowDefault();
        
    }

    private void OnEnable() {
        _view = GetComponentInParent<ViewImage>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Tween SetSpriteOpacity(int id, float opacity) {
        return _sprites[id].DOColor(new Color(1, 1, 1, opacity), 2);
    }

    public void HideSprites() {
        _seq.Kill();
        foreach (SpriteRenderer sprite in _sprites) {
            sprite.DOColor(new Color(1, 1, 1, 0), 1);
        }
    }

    public IEnumerator ShowSuccess() {
        _view.SendStringToServer("ShowText:Success");
        yield return StartSequence(new SpriteRenderer[] { spriteSaved, spriteBox, spriteBring }, 1);
        ShowDefault();
    }

    public IEnumerator ShowTryAgain() {
        _view.SendStringToServer("ShowText:Error");
        yield return StartSequence(new SpriteRenderer[] { spriteError, spriteRetry }, 2);
        ShowDefault();
    }

    public void ShowDefault() {
        _view.SendStringToServer("ShowText:Default");
        StartSequence(new SpriteRenderer[] { spriteDefault, spriteScan }, -1);
    }

    public YieldInstruction StartSequence(SpriteRenderer[] sprites, int loops) {
        HideSprites();
        _seq = DOTween.Sequence();
        _seq.SetDelay(0.5f);
        foreach (SpriteRenderer sprite in sprites) {
            _seq.AppendInterval(0.5f);
            _seq.Append(sprite.DOColor(new Color(1, 1, 1, 1), 1));
            _seq.AppendInterval(2);
            _seq.Append(sprite.DOColor(new Color(1, 1, 1, 0), 1));
        }
        _seq.SetLoops(loops);
        return _seq.WaitForCompletion();
    }

}
