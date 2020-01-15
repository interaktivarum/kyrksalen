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
                InputFinished();
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
        //light.DOIntensity(1, 1);
        Color c = new Color(0.1f, 0.2f, 1);
        globalLight.DOColor(c, 2);
        background.DOColor(c, 2);
    }

    void InputFinished() {
        GetComponentInParent<ViewPipes>().InputFinished(password);
        
        /*Color c = correct ? Color.green : Color.red;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(globalLight.DOIntensity(0,2));
        sequence.Append(globalLight.DOColor(c, 0));
        sequence.Append(globalLight.DOIntensity(3, 2));

        Sequence sequence2 = DOTween.Sequence();
        sequence2.Append(background.DOColor(Color.gray, 2));
        sequence2.Append(background.DOColor(c, 2));*/
        //background.DOColor(c, 3);
    }

}
