using Newtonsoft.Json;
using MadeNPlayShared;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppStartup : MonoBehaviour
{
    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        SetupContext(
            Directory.GetParent(Application.dataPath).FullName, 
            "environment.json"
        );
        SetupFolders();
        SetupUser();
        NetworkPrefabManager.Instanse.ResetPrefabs();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 240;

        SceneManager.LoadScene(CONSTANTS.MENU_SCENE_INDEX);
    }

    private void SetupContext(string basePath, string enviromentFileName)
    {
        var appContext = new AppContext().SetBasePath(basePath);

        try
        {
            appContext.LoadEnvironment(enviromentFileName);
        }
        catch (Exception ex)
        {
            if (ex is FileNotFoundException || ex is FormatException)
            {
                var jsonProvider = new JsonProvider();
                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new IgnoreResolver(),
                    Formatting = Formatting.Indented
                };
                var enviroment = DefaultVariables.ProjectEnviroment;
                var path = Path.Combine(basePath, enviromentFileName);
                
                jsonProvider.Save(enviroment, path, jsonSerializerSettings);
                appContext.LoadEnvironment(enviromentFileName);
            }
        }

        appContext.SetCurrent();
    }

    private void SetupFolders()
    {
        if (Directory.Exists(AppContext.Current.Enviroment.ConfigFolderPath) == false)
            Directory.CreateDirectory(AppContext.Current.Enviroment.ConfigFolderPath);

        if (Directory.Exists(AppContext.Current.Enviroment.GamesFolderPath) == false)
            Directory.CreateDirectory(AppContext.Current.Enviroment.GamesFolderPath);
    }

    private void SetupUser()
    {
        var userProvider = new UserProvider(AppContext.Current.Enviroment.ConfigFolderPath);

        try
        {
            var user = userProvider.Load();
        }
        catch (FileNotFoundException)
        {
            var user = userProvider.GetRandom();
            userProvider.Save(user);
        }
    }
}