using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LetterPipe : MonoBehaviour {
    // Start is called before the first frame update

    public string letter;
    public GameObject letterSpherePrefab;
    public float bounceDelay;
    private GameObject _sphere = null;
    private bool _creatingLetter = false;

    private TCPMessageHandler _mh;

    void Start() {

        _mh = FindObjectOfType<TCPMessageHandler>();
        GetComponentInChildren<TextMesh>().text = letter.ToUpper();

        //StartCoroutine(BounceLoop());

    }

    // Update is called once per frame
    void Update() {
        /*if (!_sphere && !_creatingLetter) {
            _creatingLetter = true;

            Sequence sequence = DOTween.Sequence();

            Tween t = transform.DOLocalMoveY(1f, 3);
            t.OnComplete(() => InitSphere());

            sequence.Append(t);
            sequence.Append(transform.DOLocalMoveY(0, 2));    
        }*/
    }

    void OnMouseDown() {
        //InitSphere();
        DropSphere2();
        _mh.SendStringToServer("LetterSelected:" + letter);
    }

    /*void InitSphere () {
        _sphere = Instantiate(letterSpherePrefab, transform);
        _sphere.GetComponentInChildren<TextMesh>().text = letter.ToUpper();
        _creatingLetter = false;
    }

    void DropSphere () {
        if (_sphere) {
            Transform freeSpheresContainer = GetComponentInParent<LetterPipes>().freeSpheresContainer.transform;
            GameObject sphere = Instantiate(_sphere, freeSpheresContainer, true);
            sphere.transform.position = _sphere.transform.position;
            sphere.GetComponentInChildren<Rigidbody>().useGravity = true;
            sphere.GetComponent<SphereCollider>().enabled = true;
            
            Destroy(_sphere);
            _sphere = null;
        }
    }*/

    void DropSphere2() {
        //if (_sphere) {
        Transform freeSpheresContainer = GetComponentInParent<LetterPipes>().freeSpheresContainer.transform;
        GameObject sphere = Instantiate(letterSpherePrefab, freeSpheresContainer, true);
        sphere.transform.position = transform.position + new Vector3(0,0,0);
        sphere.GetComponentInChildren<Rigidbody>().useGravity = true;
        sphere.GetComponent<SphereCollider>().enabled = true;
        sphere.GetComponentInChildren<TextMesh>().text = letter.ToUpper();
        //Destroy(_sphere);
        //_sphere = null;
        //}
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

    public void AnimateTo(Vector3 pos) {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(5);
        sequence.Append(transform.DOLocalMove(pos, 3f));
    }

    public void Destroy() {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(5);
        sequence.Append(transform.DOLocalMoveY(1, 3f));
        sequence.OnComplete(
            () => Destroy(this.gameObject));
    }

}
