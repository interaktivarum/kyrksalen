using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;

public class ViewIntroMovie : ViewBase
{

    VideoPlayer _video;
    bool _started = false;
    bool _exitView = false;
    public Image imageFade;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update() {
        if (!_unloading) {
            if (_video.isPlaying) {
                if (!_started) {
                    _started = true;
                }
            }
            else {
                if (_started && !_exitView) {
                    FinishedPlaying();
                }
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            if (Input.mousePosition.x < 200 && Input.mousePosition.y > 880) {
                views.Restart();
            }
        }

    }

    public override void SetReferences() {
        base.SetReferences();
        _video = GetComponentInChildren<VideoPlayer>();
    }

    public override void LoadView() {
        base.LoadView();
        _video.targetTexture.Release();
        _video.frame = 0;
        _started = false;
        _exitView = false;
        FadeText();
    }

    public void FinishedPlaying() {
        _exitView = true;
        SendStringToServer("IntroMovieFinished:true");
        InitUnloadView();
    }

    private void OnDisable() {
        _video.Stop();
    }

    void FadeText() {
        imageFade.color = Color.white;
        Sequence seq = DOTween.Sequence();
        seq.SetDelay(6);
        seq.Append(imageFade.DOColor(new Color(1, 1, 1, 0),1));
    }
}
