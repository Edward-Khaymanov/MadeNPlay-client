using ExternalResourceLoader;
using Newtonsoft.Json;
using MadeNPlayShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class GameProvider
{
    private string _gamesFolderPath;

    public GameProvider(string gamesFolderPath)
    {
        _gamesFolderPath = gamesFolderPath;
    }

    public List<LocalGameData> GetLocalGames()
    {
        var result = new List<LocalGameData>();
        var foldersPath = Directory.GetDirectories(_gamesFolderPath);

        foreach (var folderPath in foldersPath)
        {
            var gameData = GetLocalGame(folderPath);
            if (gameData == null)
                continue;

            result.Add(gameData);
        }

        return result;
    }

    public LocalGameData GetLocalGame(Guid id, string version)
    {
        var gamePath = GetGamePath(id, version);
        return GetLocalGame(gamePath);
    }

    public LocalGameData GetLocalGame(string gamePath)
    {
        var gameInfoFilePath = Path.Combine(gamePath, MadeNPlayShared.CONSTANTS.GAMEDATA_FILENAME);
        if (File.Exists(gameInfoFilePath) == false)
            return null;

        var gameData = DeserializeGameData(gameInfoFilePath);
        var iconPath = GetGameIconPath(gamePath);

        gameData.Icon = GetGameIcon(iconPath);
        gameData.Path = gamePath;

        return gameData;
    }

    public GameObject GetGameHandler(string path)
    {
        var assetLoader = new AssetLoader();
        var gameDataPath = Path.Combine(path, ExternalResourceLoader.Settings.DataFolderName);
        return assetLoader.GetGameObject(gameDataPath, MadeNPlayShared.CONSTANTS.GAME_HANDLER_LABEL);
    }

    public List<GameObject> GetNetworkPrefabs(string path)
    {
        var assetLoader = new AssetLoader();
        var gameDataPath = Path.Combine(path, ExternalResourceLoader.Settings.DataFolderName);
        var objects = assetLoader.GetGameObjects(gameDataPath, MadeNPlayShared.CONSTANTS.NETWORK_PREFAB_LABEL);
        return objects.ToList();
    }

    public string GetGamePath(Guid id, string version)
    {
        var foldersPath = Directory.GetDirectories(_gamesFolderPath);

        foreach (var folderPath in foldersPath)
        {
            var gameInfoFilePath = Path.Combine(folderPath, MadeNPlayShared.CONSTANTS.GAMEDATA_FILENAME);

            if (File.Exists(gameInfoFilePath) == false)
                continue;

            var gameData = DeserializeGameData(gameInfoFilePath);
            if (gameData.Id == id && gameData.Version == version)
                return folderPath;
        }

        return null;
    }

    public Dictionary<string, byte[]> GetFolderHash(string path)
    {
        if (Directory.Exists(path) == false)
            throw new DirectoryNotFoundException(path);

        var result = new Dictionary<string, byte[]>();
        var filesPath = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        foreach (var filePath in filesPath)
        {
            var hash = GetFileHash(filePath);
            var fileName = Path.GetFileName(filePath);
            result.Add(fileName, hash);
        }

        return result;
    }

    public string GetGameIconPath(string gamePath)
    {
        return Path.Combine(gamePath, MadeNPlayShared.CONSTANTS.ICON_FILENAME);
    }

    public Sprite GetGameIcon(string iconPath)
    {
        var iconByte = File.ReadAllBytes(iconPath);
        var texture = new Texture2D(2, 2);
        texture.LoadImage(iconByte);
        var rect = new Rect(0, 0, texture.width, texture.height);
        return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }

    public bool HashIsEqual(IDictionary<string, byte[]> hash1, IDictionary<string, byte[]> hash2)
    {
        if (hash1.Count != hash2.Count)
            return false;

        var keysIsEqual = hash1.Select(x => x.Key).SequenceEqual(hash2.Select(x => x.Key));
        if (keysIsEqual == false)
            return false;

        foreach (var fileHash in hash1)
        {
            var isEqual = hash2[fileHash.Key].SequenceEqual(fileHash.Value);
            if (isEqual == false)
                return false;
        }

        return true;
    }

    private byte[] GetFileHash(string path)
    {
        using (MD5 md5 = MD5.Create())
        {
            return md5.ComputeHash(File.OpenRead(path));
        }
    }

    private LocalGameData DeserializeGameData(string path)
    {
        var jsonProvider = new JsonProvider();
        var serializeSettings = new JsonSerializerSettings()
        {
            ContractResolver = new IgnoreResolver()
        };
        return jsonProvider.Load<LocalGameData>(path, serializeSettings);
    }
}