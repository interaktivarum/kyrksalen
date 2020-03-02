using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IronPython.Hosting;
using System.IO;

public class ViewImage : ViewBase
{

    string pathImagesFrom = Application.streamingAssetsPath + "/OCRSerial/Camera/";
    string pathImagesTo = Application.streamingAssetsPath + "/OCRSerial/OCR/";
    Paper _paper;

    private void Awake() {
        _paper = GetComponentInChildren<Paper>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //OCRSerial("3.jpg");
        //run_cmd("3.jpg");
        _paper.SetImage(pathImagesFrom, "3.jpg");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void OCRSerial(string fileName) {
        var engine = Python.CreateEngine();

        ICollection<string> searchPaths = engine.GetSearchPaths();

        //Path to the folder of greeter.py
        searchPaths.Add(@"C:\Users\micro\Documents\Developer\Unity\Nordiska Museet\Kyrksalen\Outro\Outro\Assets\StreamingAssets\OCRSerial\OCRSerial\");
        //searchPaths.Add(@Application.streamingAssetsPath + "/OCRSerial/OCRSerial/");
        
        //Path to the Python standard library
        searchPaths.Add(@"C:\Users\Mika\Documents\MyUnityGame\Assets\IronPython\Lib\");
        engine.SetSearchPaths(searchPaths);

        dynamic py = engine.ExecuteFile(@"C:\Users\micro\Documents\Developer\Unity\Nordiska Museet\Kyrksalen\Outro\Outro\Assets\StreamingAssets\OCRSerial\OCRSerial\OCRSerial.py");
        dynamic ocr = py.OCRSerial();
        Debug.Log(ocr.OCRSerial_file(fileName));
    }*/

    void run_cmd(string filename) {
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.FileName = Application.streamingAssetsPath + "/OCRSerial/OCRSerial.exe";
        p.StartInfo.Arguments = addQuotes(pathImagesFrom) + " " + addQuotes(pathImagesTo) + " " + filename;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();
        StreamReader sOut = p.StandardOutput;
        Debug.Log(sOut.ReadToEnd());
    }

    string addQuotes(string str) {
        return "\"" + str + "\"";
    }

}
