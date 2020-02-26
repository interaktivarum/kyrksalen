using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Views : MonoBehaviour {

    public App _app;
    public TCPMessageHandler _mh;

    public List<ViewBase> _views;
    int _viewId = 0;
    bool _blockScreensaver;
    public Image _fadeImage;

    private void Awake() {
        foreach (ViewBase view in GetComponentsInChildren<ViewBase>(true)) {
            view.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start() {
        _app = FindObjectOfType<App>();
        _mh = FindObjectOfType<TCPMessageHandler>();
        _mh.AddCallback("Restart", RestartCallback);
        _mh.AddCallback("UnloadView", UnloadCurrentView);

        SetReferences();

        _fadeImage.color = Color.black;
        LoadView(0);
        ResetFade();

        //Restart();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            UnloadCurrentView("");
        }
    }

    void SetReferences() {
        foreach (ViewBase view in GetComponentsInChildren<ViewBase>(true)) {
            view.gameObject.SetActive(true);
            view.SetReferences();
            _views.Add(view);
        }
    }

    void FadeToView(int id) {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_fadeImage.DOColor(new Color(0, 0, 0, 1), 2)
            .OnComplete(() => LoadView(id)));
        sequence.Append(_fadeImage.DOColor(new Color(0, 0, 0, 0), 2));
    }

    void LoadView(int id) {
        _app.SetInteraction();
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

    void UnloadCurrentView(string args) {
        GetCurrentView().InitUnloadView();
    }

    ViewBase GetCurrentView() {
        return _views[_viewId];
    }

    ViewBase GetView(int id) {
        return _views[id];
    }

    public void NextView() {
        int id = (_viewId + 1) % _views.Count;
        if (id == 0) {
            Restart();
        }
        else {
            FadeToView(id);
        }
    }

    public YieldInstruction FadeTo(Color color, int duration = 2) {
        return _fadeImage.DOColor(color, duration).WaitForCompletion();
    }

    public YieldInstruction Dim() {
        return FadeTo(new Color(0, 0, 0, 0.5f));
    }

    public YieldInstruction ResetFade() {
        return FadeTo(new Color(0, 0, 0, 0));
    }

    public void RestartCallback(string args) {
        Restart();
    }

    public void Restart() {
        _mh.SendStringToServer("App:Restart");
        FadeToView(0);
        /*_fadeImage.DOColor(new Color(0, 0, 0, 1), 2)
            .OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));*/
    }

    public void RestartScreensaver() {
        if (!_blockScreensaver && !GetCurrentView().blockScreensaver) {
            _mh.SendStringToServer("App:NoInteraction");
            Restart();
        }
    }

    public void BlockScreensaver(bool state = true) {
        _blockScreensaver = state;
    }

}