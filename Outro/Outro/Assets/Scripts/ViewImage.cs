using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ViewImage : ViewBase
{

    string pathImagesFrom = Application.streamingAssetsPath + "/OCRSerial/Camera/";
    string pathImagesTo = Application.streamingAssetsPath + "/OCRSerial/OCR/";
    Paper _paper;
    string _errorStr = "Error";
    Instructions _instructions;

    private void Awake() {
        _paper = GetComponentInChildren<Paper>();
        _instructions = GetComponentInChildren<Instructions>();
    }

    // Start is called before the first frame update
    void Start()
    {
        views._mh.AddCallback("ImageScanned", OnImageScanned);
        views._mh.AddCallback("UnloadImage", UnloadImage);
        //OCRSerial("3.jpg");
        //run_cmd("3.jpg");
        //OCRImage("3_#123456789101112#.jpg");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string run_cmd(string filename) {
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.FileName = Application.streamingAssetsPath + "/OCRSerial/OCRSerial.exe";
        p.StartInfo.Arguments = addQuotes(pathImagesFrom) + " " + addQuotes(pathImagesTo) + " " + filename;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();
        p.WaitForExit();
        Debug.Log("Exit code: " + p.ExitCode);
        return p.ExitCode == 0 ? p.StandardOutput.ReadToEnd() : _errorStr;
    }

    string addQuotes(string str) {
        return "\"" + str + "\"";
    }

    void OnImageScanned(string filename) {
        string filenameNew = run_cmd(filename);
        _instructions.HideSprites();
        if(filenameNew != _errorStr) {
            SendStringToServer("OCRSuccess:" + filenameNew);
            OCRSuccesful(filename);
        }
        else {
            SendStringToServer("OCRError:"+filename);
            OCRError();
        }
    }


    void OCRSuccesful(string filename) {
        _paper.SetImage(pathImagesTo, filename);
        _paper.Appear();
    }

     void OCRError() {
        _instructions.ShowTryAgain();
    }

    void UnloadImage(string args) {
        _paper.Disappear();
        _instructions.ShowScan();
    }

}
