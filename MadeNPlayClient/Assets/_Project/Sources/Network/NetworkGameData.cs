using MadeNPlayShared;
using System;

[Serializable]
public class NetworkGameData
{
    public NetworkGameData() { }

    public NetworkGameData(LocalGameData localGameData)
    {
        Id = localGameData.Id;
        Name = localGameData.Name;
        Version = localGameData.Version;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
}
