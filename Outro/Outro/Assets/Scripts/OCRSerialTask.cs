using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;

public struct ProcessResult {
    public bool Completed;
    public int ExitCode;
    public string output;
}

public static class OCRSerialTask
{

    public static async Task<ProcessResult> Execute(string pathImagesFrom, string pathImagesTo, string filename) {

        Process process = new Process();
        process.StartInfo.FileName = Application.streamingAssetsPath + "/OCRSerial/OCRSerial.exe";
        process.StartInfo.Arguments = addQuotes(pathImagesFrom) + " " + addQuotes(pathImagesTo) + " " + filename;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;

        ProcessResult result = new ProcessResult();

        StringBuilder outputBuilder = new StringBuilder();
        TaskCompletionSource<bool> outputCloseEvent = new TaskCompletionSource<bool>();

        process.OutputDataReceived += (s, e) =>
        {
            // The output stream has been closed i.e. the process has terminated
            if (e.Data == null) {
                outputCloseEvent.SetResult(true);
            }
            else {
                outputBuilder.AppendLine(e.Data);
            }
        };

        StringBuilder errorBuilder = new StringBuilder();
        TaskCompletionSource<bool> errorCloseEvent = new TaskCompletionSource<bool>();

        process.ErrorDataReceived += (s, e) =>
        {
            // The error stream has been closed i.e. the process has terminated
            if (e.Data == null) {
                errorCloseEvent.SetResult(true);
            }
            else {
                errorBuilder.AppendLine(e.Data);
            }
        };


        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        Task<bool> wait = WaitForExitAsync(process);
        Task<bool[]> processTask = Task.WhenAll(wait, outputCloseEvent.Task, errorCloseEvent.Task);        

        if (await Task.WhenAny(Task.Delay(10000), processTask) == processTask && wait.Result) {
            result.Completed = true;
            result.ExitCode = process.ExitCode;
            result.output = $"{outputBuilder}{errorBuilder}";
        }

        return result;
    }

    private static Task<bool> WaitForExitAsync(Process process) {
        return Task.Run(() => process.WaitForExit(10000));
    }

    static string addQuotes(string str) {
        return "\"" + str + "\"";
    }


}
