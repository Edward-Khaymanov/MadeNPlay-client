using NetcodeChat;
using MadeNPlayShared;
using System;
using System.Collections.Generic;
using UINotify;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class LobbyInitializer : NetworkBehaviour
{
    [SerializeField] private Lobby _lobbyTemplate;
    [SerializeField] private LobbyViewHandler _lobbyViewHandlerTemplate;
    [SerializeField] private NetworkNotify _networkNotifyTemplate;
    [SerializeField] private ChatHandler _chatHandlerTemplate;

    [SerializeField] private ChatWindow _chatWindow;
    [SerializeField] private LobbyView _lobbyView;

    private NetworkVariable<NetworkBehaviourReference> _lobbyReference;
    private NetworkVariable<NetworkBehaviourReference> _lobbyViewHandlerReference;
    private NetworkVariable<NetworkBehaviourReference> _chatHandlerReference;
    private NetworkVariable<FixedString4096Bytes> _serverName;
    private NetworkVariable<FixedString4096Bytes> _gameName;

    private void Awake()
    {
        _lobbyReference = new NetworkVariable<NetworkBehaviourReference>();
        _lobbyViewHandlerReference = new NetworkVariable<NetworkBehaviourReference>();
        _chatHandlerReference = new NetworkVariable<NetworkBehaviourReference>();
        _serverName = new NetworkVariable<FixedString4096Bytes>();
        _gameName = new NetworkVariable<FixedString4096Bytes>();
    }

    public void CreateLobby(ServerSettings settings, LocalGameData gameData)
    {
        var lobbyId = Guid.NewGuid();
        var lobbyData = new LobbyData(lobbyId, gameData.MaxPlayers, new List<SessionUser>(), new List<SessionTeam>());
        var networkGameData = new NetworkGameData(gameData);
        var teamsInfo = new List<TeamInfo>()
        {
            new TeamInfo(CONSTANTS.DEFAULT_TEAM_ID, CONSTANTS.DEFAULT_TEAM_NAME, int.MaxValue)
        };

        foreach (var team in gameData.Teams)
        {
            var teamInfo = new TeamInfo(team.Id, team.Name, team.MaxPlayers);
            teamsInfo.Add(teamInfo);
        }

        foreach (var teamInfo in teamsInfo)
        {
            var sessionTeam = new SessionTeam(teamInfo.Id, teamInfo.MaxPlayers);
            lobbyData.SessionTeams.Add(sessionTeam);
        }

        var started = new ConnectionManager().TryHost(gameData.Id, gameData.Version, settings.Port, lobbyData);
        if (started == false)
            return;

        var chatHandler = GameObject.Instantiate(_chatHandlerTemplate);
        var networkNotify = GameObject.Instantiate(_networkNotifyTemplate);
        var lobbyViewHandler = GameObject.Instantiate(_lobbyViewHandlerTemplate);
        var lobby = GameObject.Instantiate(_lobbyTemplate);

        lobby.ClientAuthenticated += OnClientAuthenticated;
        lobby.ClientAuthenticated += (clientId) => chatHandler.AddUser(lobby.ChatUserRequested(clientId));
        lobby.ClientDisconected += chatHandler.RemoveUser;
        lobby.GameStarting += () => lobbyViewHandler.PrepareToStartClientRpc(HELPERS.GetAllClients());
        lobby.UserAddingToTeam += lobbyViewHandler.AddUserToTeam;
        lobby.UserRemovingFromTeam += lobbyViewHandler.RemoveUserFromTeam;
        lobby.UserChangingTeam += lobbyViewHandler.ChangeTeam;

        chatHandler.NetworkObject.Spawn(true);
        networkNotify.NetworkObject.Spawn(false);
        lobbyViewHandler.NetworkObject.Spawn(true);
        lobby.NetworkObject.Spawn(false);

        _chatHandlerReference.Value = chatHandler;
        _lobbyViewHandlerReference.Value = lobbyViewHandler;
        _lobbyReference.Value = lobby;
        _serverName.Value = settings.ServerName;
        _gameName.Value = gameData.Name;

        chatHandler.Init(
            new List<ChatCommand>() {
                new ChatCommand { Predicate = "votekick", RequiredVotes = VotingType.Half, Action = lobby.Kick }
            }
        );

        lobbyViewHandler.InitServer(teamsInfo);
        lobby.Init(networkGameData, settings, lobbyData);
    }

    private void OnClientAuthenticated(ulong clientId)
    {
        InitializeClientRpc(HELPERS.GetClient(clientId));
    }

    [ClientRpc]
    private void InitializeClientRpc(ClientRpcParams clientRpcParams)
    {
        _chatHandlerReference.Value.TryGet(out ChatHandler chatHandler);
        _lobbyViewHandlerReference.Value.TryGet(out LobbyViewHandler lobbyViewHandler);
        _lobbyReference.Value.TryGet(out Lobby lobby);

        lobby.UserReadyStateChanging += lobbyViewHandler.SetUserReadyState;
        lobbyViewHandler.GameStarting += _lobbyView.PrepareToStart;
        lobbyViewHandler.UsersTeams.OnListChanged += _lobbyView.OnUsersTeamsChanged;
        _lobbyView.TeamSelected += (teamId) => lobby.SelectTeamServerRpc(teamId);
        _lobbyView.ReadyButtonClicked += () => lobby.SetReadyStateServerRpc();

        if (NetworkManager.Singleton.IsHost)
        {
            _lobbyView.StartButtonClicked += lobby.StartGame;
        }

        _chatWindow.Init(chatHandler);
        _lobbyView.Init(
            lobbyViewHandler.UsersTeams.ToList(),
            lobbyViewHandler.TeamsInfo.ToList(),
            _serverName.Value.ToString(),
            _gameName.Value.ToString()
        );
    }
}