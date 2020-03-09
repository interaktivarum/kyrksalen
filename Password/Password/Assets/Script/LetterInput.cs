using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LetterInput : MonoBehaviour
{

    public string password;
    public GameObject cap;
    public GameObject freeSpheresContainer;
    bool _isOpen = false;
    public Image background;
    public Light globalLight;

    // Start is called before the first frame update
    void Start()
    {
        CloseCap();
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

    public void ClearPassword() {
        password = "";
    }

    private void OnTriggerEnter(Collider other) {
        LetterSphere ls = other.gameObject.GetComponent<LetterSphere>();
        if (!_isOpen && password.Length < 5) {
            if (ls) {
                password += ls.GetComponentInChildren<TextMesh>().text;
                Sequence seq = DOTween.Sequence();
                seq.PrependInterval(2);
                seq.Append(other.gameObject.transform.DORotate(new Vector3(), 1));
            }
            if (password.Length >= 5) {
                InputFinished();
                //OpenCap();
            }
        }
    }

    public void OpenCap() {
        _isOpen = true;
        Sequence seq = DOTween.Sequence();
        seq.PrependInterval(3);
        seq.Append(cap.transform.DOLocalMoveX(-1, 1));
    }

    void CloseCap() {
        password = "";
        _isOpen = false;
        cap.transform.DOLocalMoveX(-0.5f, 1);
        GetComponentInParent<ViewPipes>().NewTry();
    }

    void InputFinished() {
        GetComponentInParent<ViewPipes>().InputFinished(password);
    }

}
