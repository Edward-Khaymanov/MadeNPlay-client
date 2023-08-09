using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _readyButton;
    [SerializeField] private Transform _teamsContainer;
    [SerializeField] private TeamView _teamTemplate;
    [SerializeField] private TMP_Text _serverNameSource;
    [SerializeField] private TMP_Text _gameNameSource;

    private readonly List<TeamView> _teamViews = new List<TeamView>();
    private Canvas _canvas;
    private ColorTheme _currentTheme;

    public event Action<int> TeamSelected;
    public event Action StartButtonClicked;
    public event Action ReadyButtonClicked;

    private ulong LocalClientId => NetworkManager.Singleton.LocalClientId;
    private ulong HostClientId => NetworkManager.ServerClientId;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        _currentTheme = ThemeManager.CurrentTheme;
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(() => StartButtonClicked?.Invoke());
        _readyButton.onClick.AddListener(() => ReadyButtonClicked?.Invoke());
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(() => StartButtonClicked?.Invoke());
        _readyButton.onClick.RemoveListener(() => ReadyButtonClicked?.Invoke());
    }

    public void Init(
        List<UserTeam> userTeams,
        List<TeamInfo> teamsInfo,
        string serverName,
        string gameName)
    {
        var isHost = NetworkManager.Singleton.IsHost;

        foreach (var teamInfo in teamsInfo)
        {
            var teamView = RenderTeam(teamInfo);
            _teamViews.Add(teamView);
            RenderSlots(teamView, userTeams);
        }

        _readyButton.gameObject.SetActive(isHost == false);
        _startButton.gameObject.SetActive(isHost);
        _serverNameSource.text = $"Server: {serverName}";
        _gameNameSource.text = $"Game: {gameName}";
        _canvas.enabled = true;
    }

    public void OnUsersTeamsChanged(NetworkListEvent<UserTeam> changeEvent)
    {
        var oldValue = changeEvent.PreviousValue;
        var newValue = changeEvent.Value;

        if (changeEvent.Type == NetworkListEvent<UserTeam>.EventType.Add)
        {
            AddUserToTeamView(newValue.User.ClientId, newValue.User.Name.ToString(), newValue.TeamId);
            ChangeUserReadyState(newValue.User.ClientId, newValue.TeamId, newValue.IsReady);
        }

        if (changeEvent.Type == NetworkListEvent<UserTeam>.EventType.Remove)
        {
            RemoveUserFromTeamView(newValue.User.ClientId, newValue.TeamId);
        }

        if (changeEvent.Type == NetworkListEvent<UserTeam>.EventType.Value)
        {
            if (oldValue.TeamId != newValue.TeamId)
            {
                RemoveUserFromTeamView(oldValue.User.ClientId, oldValue.TeamId);
                AddUserToTeamView(newValue.User.ClientId, newValue.User.Name.ToString(), newValue.TeamId);
            }

            ChangeUserReadyState(newValue.User.ClientId, newValue.TeamId, newValue.IsReady);
        }
    }

    public void PrepareToStart()
    {
        SetControlsState(false);
    }

    private void SetControlsState(bool state)
    {
        foreach (var view in _teamViews)
        {
            view.SetJoinButtonActive(state);
        }

        _readyButton.gameObject.SetActive(state);

        if (NetworkManager.Singleton.IsHost)
        {
            _readyButton.gameObject.SetActive(false);
            _startButton.gameObject.SetActive(state);
        }
    }

    private TeamView RenderTeam(TeamInfo team)
    {
        var view = Instantiate(_teamTemplate, _teamsContainer);
        var isDefaultTeam = team.Id == CONSTANTS.DEFAULT_TEAM_ID;
        view.Init(team, _currentTheme.LobbyTheme.TeamViewColors, isDefaultTeam);
        if (team.Id != CONSTANTS.DEFAULT_TEAM_ID)
            view.TeamRequested += (teamId) => TeamSelected?.Invoke(teamId);

        return view;
    }

    private void RenderSlots(TeamView view, List<UserTeam> userTeams)
    {
        var teamUsers = userTeams.Where(x => x.TeamId == view.TeamId);
        foreach (var teamUser in teamUsers)
        {
            var isCurrentUser = teamUser.User.ClientId == LocalClientId;
            var isHostUser = teamUser.User.ClientId == HostClientId;
            view.AddUser(teamUser.User.ClientId, teamUser.User.Name.ToString(), isCurrentUser, isHostUser);
            ChangeUserReadyState(teamUser.User.ClientId, view.TeamId, teamUser.IsReady);
        }
    }

    private void AddUserToTeamView(ulong clientId, string userName, int teamId)
    {
        var teamView = _teamViews.FirstOrDefault(x => x.TeamId == teamId);
        var isCurrentUser = clientId == LocalClientId;
        var isHostUser = clientId == HostClientId;
        teamView.AddUser(clientId, userName, isCurrentUser, isHostUser);
    }

    private void RemoveUserFromTeamView(ulong clientId, int teamId)
    {
        var teamView = _teamViews.FirstOrDefault(x => x.TeamId == teamId);
        var isCurrentUser = clientId == LocalClientId;
        teamView.RemoveUser(clientId, isCurrentUser);
    }

    private void ChangeUserReadyState(ulong clientId, int teamId, bool isReady)
    {
        var teamView = _teamViews.FirstOrDefault(x => x.TeamId == teamId);
        teamView.ChangeUserReadyState(clientId, isReady);
    }
}