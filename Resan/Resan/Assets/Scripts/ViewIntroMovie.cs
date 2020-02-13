using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ViewIntroMovie : ViewBase
{

    VideoPlayer _video;
    bool _started = false;
    bool _exitView = false;

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
        _video = GetComponentInChildren<VideoPlayer>();
    }

    public override void LoadView() {
        base.LoadView();
        _started = false;
        _exitView = false;
        _video.frame = 0;
        _video.Play();
    }

    public override void UnloadView() {
        _video.Stop();
        _video.frame = 0;
        base.UnloadView();
    }

    public void FinishedPlaying() {
        _exitView = true;
        SendStringToServer("IntroMovieFinished:true");
        UnloadView();
    }
}
