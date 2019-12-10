using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spinner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(new Vector3(0, 45, 0), 2));
        seq.AppendInterval(5);
        seq.Append(transform.DOLocalRotate(new Vector3(0, 0, 0), 2));
        seq.AppendInterval(11);
        seq.SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
