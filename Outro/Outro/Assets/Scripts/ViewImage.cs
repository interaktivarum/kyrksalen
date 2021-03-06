﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;

public class ViewImage : ViewBase
{

    string pathImagesFrom = Application.streamingAssetsPath + "/OCRSerial/Camera/";
    string pathImagesTo = Application.streamingAssetsPath + "/OCRSerial/OCR/";
    Paper _paper;
    Instructions _instructions;
    NativeArray<OCRResult> _ocrResult;

    private void Awake() {
        _paper = GetComponentInChildren<Paper>();
        _instructions = GetComponentInChildren<Instructions>();
    }

    // Start is called before the first frame update
    void Start()
    {
        views._mh.AddCallback("ShutterRelease", OnShutterRelease);
        views._mh.AddCallback("ImageScanned", OnImageScanned);
        views._mh.AddCallback("UnloadImage", UnloadImage); 
    }

    // Update is called once per frame
    void Update()
    {
    }

    async void RunOCRSerial(string filename) {
        ProcessResult result = await OCRSerialTask.Execute(pathImagesFrom, pathImagesTo, filename);
        Debug.Log(result.output);
        OnImageOCRFinished(result, filename);
    }

    void OnShutterRelease(string args) {
        _instructions.HideSprites();
    }

    void OnImageScanned(string filename) {
        _paper.SetImage(pathImagesFrom, filename);
        RunOCRSerial(filename);
    }

    void OnImageOCRFinished(ProcessResult result, string filename) {
        
        Debug.Log(result.ExitCode);
        if (result.ExitCode == 0) {
            SendStringToServer("OCRSuccess:" + result.output);
            OCRSuccesful(result.output);
        }
        else {
            SendStringToServer("OCRError:" + filename);
            OCRError();
        }
    }

    void OCRSuccesful(string filename) {
        _instructions.HideSprites();
        _paper.Appear();
    }

     void OCRError() {
        Debug.Log("OCRError");
        StartCoroutine(_instructions.ShowTryAgain());
    }

    void UnloadImage(string args) {
        _paper.Disappear();
        StartCoroutine(_instructions.ShowSuccess());
        //_instructions.ShowDefault();
    }

}
