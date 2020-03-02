using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ViewStart : ViewBase
{
    VideoPlayer _video;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            InitUnloadView();
        }
    }

    public override void SetReferences() {
        base.SetReferences();
        _video = GetComponentInChildren<VideoPlayer>();
    }

    public override void LoadView() {
        base.LoadView();
        _video.isLooping = true;
        _video.frame = 0;
        _video.targetTexture.Release();
    }

    private void OnDisable() {
        _video.Stop();
    }

}
