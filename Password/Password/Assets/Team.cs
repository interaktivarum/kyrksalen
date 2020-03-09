using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Team : MonoBehaviour
{

    public float delay;
    Vector3 _initPos;
    Sequence _seqBounce;

    private void Awake() {
        _initPos = transform.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable() {
        transform.localPosition = _initPos;
        transform.localScale = new Vector3(1, 1, 1);
        InitBounce();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() {
        GetComponentInParent<ViewTeams>().OnTeamClick(this);
    }

    void InitBounce() {
        
        Sequence seqDelay = DOTween.Sequence();
        seqDelay.SetDelay(delay).OnComplete(
            () => {
                _seqBounce.Kill();
                _seqBounce = DOTween.Sequence();
                _seqBounce.Append(transform.DOScale(0.9f, 1));
                _seqBounce.Append(transform.DOScale(1, 1));
                _seqBounce.AppendInterval(5);
                _seqBounce.SetLoops(-1);
            });
        
    }

    public void Select() {
        _seqBounce.Kill(true);
        transform.DOScale(1, 1);
        Sequence seq = DOTween.Sequence();
        seq.SetDelay(0.5f);
        seq.Append(transform.DOMoveX(0, 1));
    }

    public void NotSelected() {
        _seqBounce.Kill();
        transform.DOScale(new Vector3(), 1);
    }
}
