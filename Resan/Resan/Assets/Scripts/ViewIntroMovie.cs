using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;

public class ViewIntroMovie : ViewBase
{

    public VideoPlayer _video;
    public VideoPlayer _videoSub;
    public RawImage _imageSub;
    bool _started = false;
    bool _exitView = false;
    public Image imageFade;
    public Image imageBack;
    public VideoClip[] subtitles;

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

        if (Input.GetMouseButtonDown(0) && imageBack.color == Color.white) {
            if (Input.mousePosition.x < 200 && Input.mousePosition.y > 880) {
                _video.GetTargetAudioSource(0).DOFade(0, 2);
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
        
        //SetSubtitles();
        _started = false;
        _exitView = false;
        FadeText(imageFade,6);
        FadeText(imageBack, 15);
        _video.GetTargetAudioSource(0).volume = 1f;
        //GetComponentInChildren<Canvas>().sortingOrder = 10;
    }

    /*public void SetSubtitles() {
        _imageSub.enabled = false;
        _videoSub.targetTexture.Release();
        SetSubtitles(views._app.GetComponentInChildren<SubtitlesHandler>().language);
    }

    public void SetSubtitles(int id) {
        _videoSub.Stop();
        if (id >= 0) {
            _imageSub.enabled = true;
            _videoSub.clip = subtitles[id];
            _videoSub.Play();
            _videoSub.time = _video.time;
        }
    }*/

    public void FinishedPlaying() {
        _exitView = true;
        SendStringToServer("IntroMovieFinished:true");
        InitUnloadView();
    }

    private void OnDisable() {
        _video.Stop();
        //GetComponentInChildren<Canvas>().sortingOrder = 0;
    }

    void FadeText(Image image, int delay) {
        //DOTween.Kill(image.gameObject);
        image.color = Color.white;
        Sequence seq = DOTween.Sequence();
        seq.SetDelay(delay);
        seq.Append(image.DOColor(new Color(1, 1, 1, 0),1));
        AddSequence(seq);
    }
}
