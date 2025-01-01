using System.IO;
using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class BuildScript
{
    public static void Build() 
    {
        string outputPath = @"C:\Users\qkr38\Builds\Pizza.exe";
        string directory = Path.GetDirectoryName(outputPath);

        if (string.IsNullOrEmpty(outputPath) || Path.IsPathRooted(outputPath) == false)
        {
            Debug.LogError("Invalid output path: " + outputPath);
            return;
        }

        if (string.IsNullOrEmpty(directory))
        {
            Debug.LogError("Output directory could not be determined.");
            return;
        }

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var buildReport = BuildPipeline.BuildPlayer(
            new[] { "Assets/Scenes/Play Scene.unity" },
            outputPath,
            BuildTarget.StandaloneWindows64,
            BuildOptions.None
        );

        if (buildReport.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + outputPath);
        }
        else
        {
            Debug.LogError($"Build failed with {buildReport.summary.totalErrors} errors: {buildReport.summary.result}");
        }
    }


    public static void RunTest()
    {
        var testRunnerApi = new UnityEditor.TestTools.TestRunner.Api.TestRunnerApi();
        var executionSetting = new UnityEditor.TestTools.TestRunner.Api.ExecutionSettings();

        executionSetting.filters = new[] { new UnityEditor.TestTools.TestRunner.Api.Filter() };

        testRunnerApi.Execute(executionSetting);


            
    }
}