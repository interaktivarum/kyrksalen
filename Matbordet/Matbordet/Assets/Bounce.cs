using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bounce : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1.1f, 1));
        seq.Append(transform.DOScale(1, 1));
        seq.SetLoops(-1);
    }

    private void OnEnable() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
