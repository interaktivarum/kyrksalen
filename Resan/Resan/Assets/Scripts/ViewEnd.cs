using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ViewEnd : ViewBase
{

    public VideoPlayer _video;
    public VideoPlayer _videoSub;
    public RawImage _imageSub;
    bool _started = false;
    bool _exitView = false;
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
    }

    public override void SetReferences() {
        base.SetReferences();
    }

    public override void LoadView() {
        base.LoadView();
        _video.targetTexture.Release();
        _video.frame = 0;
        _videoSub.targetTexture.Release();
        SetSubtitles();
        _videoSub.targetTexture.Release();
        SetSubtitles();
        _started = false;
        _exitView = false;
        GetComponentInChildren<Canvas>().sortingOrder = 10;
    }

    public void SetSubtitles() {
        _imageSub.enabled = false;
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
    }

    public void FinishedPlaying() {
        _exitView = true;
        SendStringToServer("IntroMovieFinished:true");
        InitUnloadView();
    }

    private void OnDisable() {
        _video.Stop();
        GetComponentInChildren<Canvas>().sortingOrder = 0;
    }

}
