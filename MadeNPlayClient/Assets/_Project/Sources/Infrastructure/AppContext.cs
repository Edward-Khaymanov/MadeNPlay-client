using Newtonsoft.Json;
using MadeNPlayShared;
using System.IO;

public class AppContext
{
    public static AppContext Current { get; private set; }

    public string BasePath { get; private set; }
    public ProjectEnvironment Enviroment { get; private set; }

    public AppContext SetCurrent()
    {
        Current = this;
        return this;
    }

    public AppContext SetBasePath(string basePath)
    {
        if (Current == this)
            return this;

        BasePath = basePath;
        return this;
    }

    public AppContext LoadEnvironment(string relativeFilePath)
    {
        if (Current == this)
            return this;

        var jsonProvider = new JsonProvider();
        var path = Path.Combine(BasePath, relativeFilePath);
        var jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new IgnoreResolver(),
            MissingMemberHandling = MissingMemberHandling.Error
        };
        Enviroment = jsonProvider.Load<ProjectEnvironment>(path, jsonSerializerSettings);

        return this;
    }
}