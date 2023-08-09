using MadeNPlayShared;
using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class LobbyViewHandler : NetworkBehaviour
{
    public NetworkList<TeamInfo> TeamsInfo { get; private set; }
    public NetworkList<UserTeam> UsersTeams { get; private set; }

    public event Action GameStarting;

    private void Awake()
    {
        TeamsInfo = new NetworkList<TeamInfo>();
        UsersTeams = new NetworkList<UserTeam>();
    }
    
    public void InitServer(List<TeamInfo> teamsInfo)
    {
        TeamsInfo.AddRange(teamsInfo);
    }
    
    public void AddUserToTeam(NetworkUser user, int teamId)
    {
        var newUserTeam = new UserTeam(user, teamId, false);
        UsersTeams.Add(newUserTeam);
    }
    
    public void RemoveUserFromTeam(ulong clientId)
    {
        var userTeam = UsersTeams.FirstOrDefault(x => x.User.ClientId == clientId);
        UsersTeams.Remove(userTeam);
    }
    
    public void ChangeTeam(ulong clientId, int newTeamId)
    {
        var userTeam = UsersTeams.FirstOrDefault(x => x.User.ClientId == clientId);
        var changedUserTeam = new UserTeam(userTeam.User, newTeamId, false);
        UpdateUserTeam(userTeam, changedUserTeam);
    }
    
    public void SetUserReadyState(ulong clientId, bool isReady)
    {
        var userTeam = UsersTeams.FirstOrDefault(x => x.User.ClientId == clientId);
        var changedUserTeam = new UserTeam(userTeam.User, userTeam.TeamId, isReady);
        UpdateUserTeam(userTeam, changedUserTeam);
    }

    private void UpdateUserTeam(UserTeam oldValue, UserTeam newValue)
    {
        var index = UsersTeams.IndexOf(oldValue);
        UsersTeams[index] = newValue;
    }

    [ClientRpc]
    public void PrepareToStartClientRpc(ClientRpcParams clientRpc)
    {
        GameStarting?.Invoke();
    }
}