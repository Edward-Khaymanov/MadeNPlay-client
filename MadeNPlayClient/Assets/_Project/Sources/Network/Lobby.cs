using LobbySystem;
using NetcodeChat;
using MadeNPlayShared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UINotify;
using Unity.Netcode;
using UnityEngine;
using System.Reflection;

[DisallowMultipleComponent]
public partial class Lobby : NetworkBehaviour
{
    private ServerDiscovery _serverDiscovery;
    private LobbyData _lobbyData;
    private NetworkGameData _networkGameData;
    private ServerSettings _serverSettings;
    private SessionUser _hostUser;

    public event Action GameStarting;

    private NetworkLobbyData NetworkLobbyData => new NetworkLobbyData(
        _lobbyData.LobbyId,
        _serverSettings.ServerName,
        _serverSettings.Port,
        _lobbyData.CurrentPlayers,
        _lobbyData.MaxPlayers,
        _networkGameData
        );

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (base.NetworkManager.IsServer)
        {
            base.NetworkManager.OnClientConnectedCallback += OnClientConnected;
            base.NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;
            base.NetworkManager.SceneManager.OnLoadComplete += OnLoadedScene;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (base.NetworkManager.IsServer)
        {
            StopAdvertiseServer();
            base.NetworkManager.OnClientConnectedCallback -= OnClientConnected;
            base.NetworkManager.OnClientDisconnectCallback -= OnClientDisconnected;
            base.NetworkManager.SceneManager.OnLoadComplete -= OnLoadedScene;
        }
    }

    public void Init(NetworkGameData networkGameData, ServerSettings serverSettings, LobbyData lobbyData)
    {
        _serverDiscovery = new ServerDiscovery(AppContext.Current.Enviroment.ApplicationKey);
        _networkGameData = networkGameData;
        _serverSettings = serverSettings;
        _lobbyData = lobbyData;

        if (NetworkManager.Singleton.IsHost)
        {
            _hostUser = _lobbyData.SessionUsers.FirstOrDefault(x => x.User.ClientId == NetworkManager.ServerClientId);
            AuthenticateClient(NetworkManager.ServerClientId);
        }

        AdvertiseServer();
    }

    public void StartGame()
    {
        var defaultTeam = _lobbyData.SessionTeams.FirstOrDefault(x => x.Id == CONSTANTS.DEFAULT_TEAM_ID);
        _lobbyData.SetState(LobbyState.Starting);

        if (defaultTeam.UsersCount > 0)
        {
            LocalNotify.Show("Not all players have chosen a team", 2, NotificationStyleType.Error);
            _lobbyData.SetState(LobbyState.Waiting);
            return;
        }

        if (UsersInState(UserState.ReadyToStart, _lobbyData.SessionUsers.Except(new[] { _hostUser })) == false)
        {
            LocalNotify.Show("Not all players is ready", 2, NotificationStyleType.Error);
            _lobbyData.SetState(LobbyState.Waiting);
            return;
        }

        GameStarting?.Invoke();
        //Debug.LogWarning("All players are ready");

        StopAdvertiseServer();
        StartCoroutine(StartCountDown());
    }

    private void AdvertiseServer()
    {
        _serverDiscovery.StartServer(() => NetworkLobbyData);
    }

    private void StopAdvertiseServer()
    {
        _serverDiscovery.StopServer();
    }

    private void LoadScene()
    {
        base.NetworkManager.SceneManager.LoadScene(CONSTANTS.GAME_SCENE_NAME, UnityEngine.SceneManagement.LoadSceneMode.Single);
        StartCoroutine(CheckLoadedScene());
    }

    private void SpawnGame()
    {
        var gameProvider = new GameProvider(AppContext.Current.Enviroment.GamesFolderPath);
        var gamePath = gameProvider.GetGamePath(_networkGameData.Id, _networkGameData.Version);
        var gameHandler = gameProvider.GetGameHandler(gamePath);
        var gameHandlerInstance = Instantiate(gameHandler);
        gameHandlerInstance.GetComponent<NetworkObject>().Spawn();
        SetupGame(gameHandlerInstance);
    }

    private void SetupGame(GameObject gameHandlerInstance)
    {
        var defaultTeam = _lobbyData.SessionTeams.FirstOrDefault(x => x.Id == CONSTANTS.DEFAULT_TEAM_ID);
        _lobbyData.SessionTeams.Remove(defaultTeam);
        var asddd = AppDomain.CurrentDomain.GetAssemblies();
        var ass = asddd.FirstOrDefault(x => x.FullName.Contains("xample"));
        var t = ass.GetType("SetupLevel");
        var met = t.GetMethod("SetupGame");
        var comp = gameHandlerInstance.GetComponent(t);
        met.Invoke(comp, new object[] { _lobbyData });
        //gameHandlerInstance.SendMessage("SetupGame", _lobbyData);
        _lobbyData.SetState(LobbyState.Started);
    }

    private bool UsersInState(UserState state, IEnumerable<SessionUser> targetUsers)
    {
        return targetUsers.All(x => x.State == state);
    }

    private IEnumerator StartCountDown()
    {
        var waitSeconds = 5;
        while (waitSeconds > 0)
        {
            NetworkNotify.Instance.PushClientRPC(waitSeconds.ToString(), 1, targetClients: HELPERS.GetAllClients());
            waitSeconds -= 1;
            yield return new WaitForSecondsRealtime(1f);
        }

        NetworkNotify.Instance.PushClientRPC("Start", 1, targetClients: HELPERS.GetAllClients());
        LoadScene();
    }

    private IEnumerator CheckLoadedScene()
    {
        var waitSeconds = 60;
        while (waitSeconds > 0)
        {
            //Debug.Log($"Remaining time: {waitSeconds}");
            if (UsersInState(UserState.LoadedScene, _lobbyData.SessionUsers))
            {
                SpawnGame();
                yield break;
            }

            waitSeconds -= 1;
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    public void Kick(ulong clientId)
    {
        if (clientId == ulong.MaxValue || clientId == NetworkManager.ServerClientId)
            return;

        NetworkManager.Singleton.DisconnectClient(clientId, "Voting");
        OnClientDisconnected(clientId);
    }

    public ChatUser ChatUserRequested(ulong clientId)
    {
        var sessionUser = _lobbyData.SessionUsers.FirstOrDefault(x => x.User.ClientId == clientId);
        var chatUser = new ChatUser()
        {
            NetworkClieintId = clientId,
            Name = sessionUser.User.Name.ToString(),
            Color = UnityEngine.Random.ColorHSV()
        };
        return chatUser;
    }
}