using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Letter : MonoBehaviour {
    // Start is called before the first frame update

    public string letter;
    public GameObject letterSpherePrefab;
    public float bounceDelay;
    private GameObject _sphere = null;
    private bool _creatingLetter = false;

    private TCPMessageHandler _mh;

    void Start() {

        _mh = FindObjectOfType<TCPMessageHandler>();

        StartCoroutine(BounceLoop());
        
    }

    // Update is called once per frame
    void Update() {
        if (!_sphere && !_creatingLetter) {
            _creatingLetter = true;

            Sequence sequence = DOTween.Sequence();

            Tween t = transform.DOLocalMoveY(1f, 3);
            t.OnComplete(() => InitSphere());

            sequence.Append(t);
            sequence.Append(transform.DOLocalMoveY(0, 2));
            
        }
    }

    void OnMouseDown() {
        DropSphere();
        _mh.SendStringToServer("LetterSelected:" + letter);
    }

    void InitSphere () {
        _sphere = Instantiate(letterSpherePrefab, transform);
        _sphere.GetComponentInChildren<TextMesh>().text = letter;
        _creatingLetter = false;
    }

    void DropSphere () {
        if (_sphere) {
            Transform freeSpheresContainer = GetComponentInParent<Letters>().freeSpheresContainer.transform;
            GameObject newSphere = Instantiate(_sphere, freeSpheresContainer, true);
            newSphere.transform.position = _sphere.transform.position;
            newSphere.GetComponentInChildren<Rigidbody>().useGravity = true;
            newSphere.GetComponent<SphereCollider>().enabled = true;
            Destroy(_sphere);
            _sphere = null;
        }
    }

    IEnumerator BounceLoop() {
        while (true) {
            if (_sphere && !_creatingLetter) {
                Bounce();
            }
            yield return new WaitForSeconds(10);
        }
    }

    void Bounce() {
        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(bounceDelay);
        sequence.Append(transform.DOLocalMoveY(0.1f, 0.5f));
        sequence.Append(transform.DOLocalMoveY(0, 1f));
    }

}
