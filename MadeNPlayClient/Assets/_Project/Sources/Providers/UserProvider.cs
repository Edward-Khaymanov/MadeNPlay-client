using System;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class UserProvider
{
    private readonly string _configFolderPath;

    public UserProvider(string configFolderPath)
    {
        _configFolderPath = configFolderPath;
    }

    private string _userDataFilePath => Path.Combine(_configFolderPath, "user.json");

    public User Load()
    {
        var json = File.ReadAllText(_userDataFilePath);
        return JsonUtility.FromJson<User>(json);
    }

    public void Save(User user)
    {
        var userJson = JsonUtility.ToJson(user, true);
        File.WriteAllText(_userDataFilePath, userJson);
    }

    public User GetRandom()
    {
        var user = new User((ulong)Random.Range(0, int.MaxValue), CreateRandomString());
        return user;
    }

    private string CreateRandomString(int stringLength = 10)
    {
        var _stringLength = stringLength - 1;
        var randomString = "";
        var characters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        for (int i = 0; i <= _stringLength; i++)
        {
            randomString = randomString + characters[Random.Range(0, characters.Length)];
        }
        return randomString;
    }
}