using System.IO;
using UnityEngine;

public static class DefaultVariables
{
    public static ProjectEnvironment ProjectEnviroment = 
        new ProjectEnvironment(
            "qwwqDWADiuhd2@hud", 
            Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Configs"),
            Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Games"));
}
