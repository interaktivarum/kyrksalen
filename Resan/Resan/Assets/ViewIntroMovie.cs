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
        _video = GetComponentInChildren<VideoPlayer>();
    }

    // Update is called once per frame
    void Update() {
        if (_video.isPlaying) {
            if (!_started) {
                _started = true;
            }
        }
        else {
            if (_started && !_exitView) {
                UnloadView();
            }
        }
    }

    public override void UnloadView() {
        _exitView = true;
        base.UnloadView();

    }
}
