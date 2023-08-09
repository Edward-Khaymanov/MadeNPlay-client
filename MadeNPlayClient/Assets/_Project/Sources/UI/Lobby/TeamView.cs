using System;
using TMPro;
using UnityEngine;

public class TeamView : MonoBehaviour
{
    [SerializeField] private UsersContainer _usersContainer;
    [SerializeField] private IconButton _joinButton;
    [SerializeField] private IconButton _dropdownButton;
    [SerializeField] private TMP_Text _teamNameSource;
    [SerializeField] private TMP_Text _playersInfo;
    [SerializeField] private Sprite _joinedIcon;
    [SerializeField] private Sprite _notJoinedIcon;

    private TeamViewColors _colors;
    public event Action<int> TeamRequested;

    public TeamInfo TeamInfo { get; private set; }
    public int TeamId => TeamInfo.Id;
    public int CurrentPlayersCount => _usersContainer.Users.Count;

    public void Init(TeamInfo teamInfo, TeamViewColors colors, bool isDefaultTeam)
    {
        TeamInfo = teamInfo;
        _colors = colors;
        _teamNameSource.text = teamInfo.Name.ToString();
        RenderJoinIcon(false);
        RenderPlayersInfo();

        if (isDefaultTeam)
        {
            SetJoinButtonActive(false);
            _playersInfo.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _joinButton.Click += () => TeamRequested?.Invoke(TeamId);
    }

    private void OnDisable()
    {
        _joinButton.Click -= () => TeamRequested?.Invoke(TeamId);
    }

    public void SetJoinButtonActive(bool isActive)
    {
        _joinButton.gameObject.SetActive(isActive);
    }

    public void ChangeUserReadyState(ulong clientId, bool isReady)
    {
        _usersContainer.ChangeUserReadyState(clientId, isReady);
    }

    public void AddUser(ulong clientId, string userName, bool isCurrentUser, bool isHost)
    {
        if (isCurrentUser)
            RenderJoinIcon(true);

        _usersContainer.AddUser(clientId, userName, isCurrentUser, isHost);
        RenderPlayersInfo();
    }

    public void RemoveUser(ulong clientId, bool isCurrentUser)
    {
        if (isCurrentUser)
            RenderJoinIcon(false);

        _usersContainer.RemoveUser(clientId);
        RenderPlayersInfo();
    }

    private void RenderJoinIcon(bool isJoined)
    {
        if (isJoined)
        {
            _joinButton.Render(null, _joinedIcon);
            _joinButton.RenderColors(_colors.JoinButtonBackplateJoined, _colors.JoinButtonIconJoined);
        }
        else
        {
            _joinButton.Render(null, _notJoinedIcon);
            _joinButton.RenderColors(_colors.JoinButtonBackplateDefault, _colors.JoinButtonIconDefault);
        }
    }

    private void RenderPlayersInfo()
    {
        _playersInfo.text = $"{CurrentPlayersCount}/{TeamInfo.MaxPlayers}";
    }
}