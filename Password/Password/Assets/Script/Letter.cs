using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Letter : MonoBehaviour {
    // Start is called before the first frame update

    public string letter;
    public GameObject letterSpherePrefab;
    public GameObject freeSpheresContainer;
    private GameObject _sphere = null;
    private bool _creatingLetter = false;

    void Start() {
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
    }

    void InitSphere () {
        _sphere = Instantiate(letterSpherePrefab, transform);
        _sphere.GetComponentInChildren<TextMesh>().text = letter;
        _creatingLetter = false;
    }

    void DropSphere () {
        if (_sphere) {
            GameObject newSphere = Instantiate(_sphere, freeSpheresContainer.transform, true);
            newSphere.transform.position = _sphere.transform.position;
            newSphere.GetComponentInChildren<Rigidbody>().useGravity = true;
            newSphere.GetComponent<SphereCollider>().enabled = true;
            Destroy(_sphere);
            _sphere = null;
        }
    }

}
