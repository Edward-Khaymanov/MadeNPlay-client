using System;
using UnityEngine;

[Serializable]
public class LobbyTheme
{
    [SerializeField] private TeamViewColors _teamViewColors;
    [SerializeField] private UserViewColors _userViewColors;

    public TeamViewColors TeamViewColors => _teamViewColors;
    public UserViewColors UserViewColors => _userViewColors;
}

[Serializable]
public class TeamViewColors
{
    [field: SerializeField] public Color Backplate { get; private set; }
    [field: SerializeField] public Color Text { get; private set; }

    [field: SerializeField] public Color JoinButtonIconDefault { get; private set; }
    [field: SerializeField] public Color JoinButtonIconJoined { get; private set; }
    [field: SerializeField] public Color JoinButtonBackplateDefault { get; private set; }
    [field: SerializeField] public Color JoinButtonBackplateJoined { get; private set; }

    [field: SerializeField] public Color DropdownBackplate { get; private set; }
    [field: SerializeField] public Color DropdownIcon { get; private set; }
}

[Serializable]
public class UserViewColors
{
    [field: SerializeField] public Color CurrentUserBackplate { get; private set; }
    [field: SerializeField] public Color Backplate { get; private set; }
    [field: SerializeField] public Color Text { get; private set; }
    [field: SerializeField] public Color ReadyIconReady { get; private set; }
    [field: SerializeField] public Color ReadyIconNotReady { get; private set; }
    [field: SerializeField] public Color ReadyIconBackplateReady { get; private set; }
    [field: SerializeField] public Color ReadyIconBackplateNotReady { get; private set; }
    [field: SerializeField] public Color HostIcon { get; private set; }
    [field: SerializeField] public Color HostIconBackplate { get; private set; }
}