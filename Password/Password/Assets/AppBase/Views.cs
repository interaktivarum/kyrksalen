using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Views : MonoBehaviour
{

    public TCPMessageHandler _mh;

    public List<ViewBase> _views;
    int _viewId = 0;
    public Image _fadeImage;

    // Start is called before the first frame update
    void Start()
    {
        _mh = FindObjectOfType<TCPMessageHandler>();
        _mh.AddCallback("Restart", RestartCallback);

        SetReferences();

        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GetCurrentView().UnloadView();
        }
    }

    void SetReferences() {
        foreach(ViewBase view in GetComponentsInChildren<ViewBase>(true)) {
            view.gameObject.SetActive(true);
            view.SetReferences();
            _views.Add(view);
        }
    }

    void LoadView(int id) {
        _viewId = id;
        LoadCurrentView();
    }

    void LoadCurrentView() {
        if (_viewId < transform.Find("Views").childCount) {
            foreach (ViewBase view in _views) {
                view.gameObject.SetActive(false);
            }
            GetCurrentView().LoadView();
            _mh.SendStringToServer("ViewLoaded:" + GetCurrentView().name);
        }
    }

    ViewBase GetCurrentView() {
        return _views[_viewId];
    }

    ViewBase GetView(int id) {
        return _views[id];
    }

    public void NextView() {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_fadeImage.DOColor(new Color(0, 0, 0, 1), 2)
            .OnComplete(() => LoadView((_viewId + 1) % _views.Count)));
        sequence.Append(_fadeImage.DOColor(new Color(0, 0, 0, 0), 2));
    }

    public YieldInstruction FadeTo(int val) {
        return _fadeImage.DOColor(new Color(0, 0, 0, val), 2).WaitForCompletion();
    }

    public void RestartCallback(string args) {
        Restart();
    }

    public void Restart() {
        LoadView(0);
    }

}
