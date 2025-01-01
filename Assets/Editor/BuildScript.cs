using System.IO;
using System;
using UnityEditor;

public class BuildScript
{
    public static void Build() 
    {
        string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Pizza.exe");
        BuildPipeline.BuildPlayer(new[] { "Assets/Scenes/PlayScene.unity" }, outputPath, BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    public static void RunTest()
    {
        var testRunnerApi = new UnityEditor.TestTools.TestRunner.Api.TestRunnerApi();
        var executionSetting = new UnityEditor.TestTools.TestRunner.Api.ExecutionSettings();

        executionSetting.filters = new[] { new UnityEditor.TestTools.TestRunner.Api.Filter() };

        testRunnerApi.Execute(executionSetting);


            
    }
}