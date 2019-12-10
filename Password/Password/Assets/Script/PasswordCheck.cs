using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PasswordCheck : MonoBehaviour
{

    public string password;
    public GameObject cap;
    public GameObject freeSpheresContainer;
    bool _isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isOpen) {
            if(freeSpheresContainer.transform.childCount == 0) {
                CloseCap();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        LetterSphere ls = other.gameObject.GetComponent<LetterSphere>();
        if (!_isOpen) {
            if (ls) {
                password += ls.GetComponentInChildren<TextMesh>().text;
                Sequence seq = DOTween.Sequence();
                seq.PrependInterval(2);
                seq.Append(other.gameObject.transform.DORotate(new Vector3(), 1));
            }
            if (password.Length >= 5) {
                OpenCap();
            }
        }
    }

    void OpenCap() {
        _isOpen = true;
        Sequence seq = DOTween.Sequence();
        seq.PrependInterval(3);
        seq.Append(cap.transform.DOLocalMoveX(-1, 1));
    }

    void CloseCap() {
        password = "";
        _isOpen = false;
        cap.transform.DOLocalMoveX(-0.5f, 1);
    }

}
