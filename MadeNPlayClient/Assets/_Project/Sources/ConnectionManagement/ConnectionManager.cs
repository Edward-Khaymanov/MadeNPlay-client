using ExternalResourceLoader;
using MadeNPlayShared;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class ConnectionManager
{
    public bool TryConnect(
        Guid gameId,
        string gameVersion,
        string ipAddress,
        ushort port)
    {
        var authData = GetAuthData();
        InitializeGame(gameId, gameVersion);
        InitNetwork(authData, ipAddress, port, null);
        var connected = NetworkManager.Singleton.StartClient();
        return connected;
    }

    public bool TryHost(
        Guid gameId,
        string gameVersion,
        ushort port,
        LobbyData lobbyData,
        string listenAddress = "0.0.0.0")
    {
        var authData = GetAuthData();
        InitializeGame(gameId, gameVersion);
        NetworkManager.Singleton.GetComponent<Authenticator>().Init(lobbyData);
        InitNetwork(authData, "127.0.0.1", port, listenAddress);
        var started = NetworkManager.Singleton.StartHost();
        return started;
    }

    private void InitNetwork(AuthData authData, string ipAddress, ushort port, string listenAddress)
    {
        var authDataJson = JsonUtility.ToJson(authData);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress, port, listenAddress);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(authDataJson);
    }

    private AuthData GetAuthData()
    {
        var userProvider = new UserProvider(AppContext.Current.Enviroment.ConfigFolderPath);
        var user = userProvider.Load();
        var authData = new AuthData()
        {
            UserId = user.Id,
            UserName = user.Name,
        };

        return authData;
    }

    private void InitializeGame(Guid id, string version)
    {
        var gameProvider = new GameProvider(AppContext.Current.Enviroment.GamesFolderPath);
        var gamePath = gameProvider.GetGamePath(id, version);

        var dllLoader = new DllLoader();
        var dllpath = dllLoader.GetDllFilePath(gamePath);
        dllLoader.Load(dllpath);

        var gameHandler = gameProvider.GetGameHandler(gamePath);
        SetGlobalObjectIdHash(gameHandler);
        NetworkManager.Singleton.AddNetworkPrefab(gameHandler);
        var networkPrefabs = gameProvider.GetNetworkPrefabs(gamePath);
        networkPrefabs = networkPrefabs.Distinct().ToList();
        foreach (var prefab in networkPrefabs)
        {
            SetGlobalObjectIdHash(prefab);
            NetworkManager.Singleton.AddNetworkPrefab(prefab);
        }
    }

    private void SetGlobalObjectIdHash(GameObject target)
    {
        var networkObject = target.GetComponent<NetworkObject>();
        var networkId = target.GetComponent<NetworkId>().Id;
        var hashField = typeof(NetworkObject).GetField("GlobalObjectIdHash", BindingFlags.Instance | BindingFlags.NonPublic);
        hashField.SetValue(networkObject, networkId);
        //Debug.Log($"{target.name}---{networkId}");
    }
}