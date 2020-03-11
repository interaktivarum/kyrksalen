using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct OCRResult {

    public OCRResult(string o) {
        output = o;
    }

    public void SetOutput(string o) {
        output = o;
    }

    [MarshalAs(UnmanagedType.LPStr)] public string output;

}

public struct OCRSerial : IJob
{
    public NativeArray<OCRResult> Result;

    public string pathImagesFrom;
    public string pathImagesTo;
    public string filename;
    public string _errorStr;

    public void Execute() {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.FileName = Application.streamingAssetsPath + "/OCRSerial/OCRSerial.exe";
        process.StartInfo.Arguments = addQuotes(pathImagesFrom) + " " + addQuotes(pathImagesTo) + " " + filename;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        process.WaitForExit();
        Debug.Log("Exit code: " + process.ExitCode);
        string output = process.ExitCode == 0 ? process.StandardOutput.ReadToEnd() : _errorStr;
        //Result[0] = new OCRResult(output);
        Result[0].SetOutput(output);
    }

    string addQuotes(string str) {
        return "\"" + str + "\"";
    }

}
