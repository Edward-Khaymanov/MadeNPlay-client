using System;

[Serializable]
public struct ProjectEnvironment
{
    public ProjectEnvironment(string applicationKey, string configFolderPath, string gamesFolderPath)
    {
        ApplicationKey = applicationKey;
        ConfigFolderPath = configFolderPath;
        GamesFolderPath = gamesFolderPath;
    }

    public string ApplicationKey { get; private set; }
    public string ConfigFolderPath { get; private set; }
    public string GamesFolderPath { get; private set; }
}