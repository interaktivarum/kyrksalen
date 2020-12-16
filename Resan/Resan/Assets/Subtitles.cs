using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Subtitles : MonoBehaviour
{
    public Sprite[] flags;
    public VideoClip[] subtitles;
    public VideoPlayer videoFollow;
    RawImage _imageSubtitles;
    Image _imageFlag;
    VideoPlayer _video;
    SubtitlesHandler _handler;

    // Start is called before the first frame update
    void Start()
    {
        _imageSubtitles = GetComponentInChildren<RawImage>(true);
        _video = GetComponentInChildren<VideoPlayer>(true);
        _handler = FindObjectOfType<SubtitlesHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {

        _imageSubtitles = GetComponentInChildren<RawImage>(true);
        _imageFlag = GetComponentInChildren<Image>(true);
        _video = GetComponentInChildren<VideoPlayer>(true);
        _handler = FindObjectOfType<SubtitlesHandler>();

        _imageSubtitles.enabled = false;
        SetSubtitles();
        GetComponentInChildren<Canvas>().sortingOrder = 1;
    }

    private void OnDisable() {
        GetComponentInChildren<Canvas>().sortingOrder = 0;
    }

    public void SwitchLanguage() {
        SetSubtitles(_handler.languageOpposite());
    }

    public void SetSubtitles() {
        if (_handler) {
            SetSubtitles(_handler.language);
        }
    }

    public void SetSubtitles(int id) {

        //Update handler
        _handler.SetLanguage(id);

        //Set flag
        _imageFlag.sprite = flags[_handler.languageOpposite()];

        if (subtitles.Length > id) {
            //Set video clip
            _video.targetTexture.Release();
            _video.clip = subtitles[id];
            _video.Play();
            _video.time = videoFollow.time;

            //Show render image
            _imageSubtitles.enabled = true;
        }
    }

}
