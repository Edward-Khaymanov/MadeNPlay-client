using MadeNPlayShared;
using System;
using System.Linq;
using UINotify;
using Unity.Netcode;

public partial class Lobby : NetworkBehaviour
{
    public event Action<NetworkUser, int> UserAddingToTeam;
    public event Action<ulong> UserRemovingFromTeam;
    public event Action<ulong, int> UserChangingTeam;
    public event Action<ulong, bool> UserReadyStateChanging;

    public void AddUserToTeam(SessionUser sessionUser, SessionTeam team)
    {
        sessionUser.Team = team;
        team.Users.Add(sessionUser);
        UserAddingToTeam?.Invoke(sessionUser.User, team.Id);
    }

    public void RemoveUserFromTeam(SessionUser sessionUser)
    {
        sessionUser.Team.Users.Remove(sessionUser);
        sessionUser.Team = null;
        UserRemovingFromTeam?.Invoke(sessionUser.User.ClientId);
    }

    public void ChangeTeam(SessionUser sessionUser, SessionTeam newTeam)
    {
        sessionUser.State = UserState.Authenticated;
        sessionUser.Team.Users.Remove(sessionUser);
        sessionUser.Team = newTeam;
        newTeam.Users.Add(sessionUser);
        UserChangingTeam?.Invoke(sessionUser.User.ClientId, newTeam.Id);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SelectTeamServerRpc(int teamId, ServerRpcParams serverRpc = default)
    {
        if (_lobbyData.State != LobbyState.Waiting)
        {
            NetworkNotify.Instance.PushClientRPC(
                $"You can't change team when game is {_lobbyData.State}",
                1,
                NotificationStyleType.Error,
                targetClients: HELPERS.GetClient(serverRpc.Receive.SenderClientId));
            return;
        }

        var sessionUser = _lobbyData.SessionUsers.FirstOrDefault(x => x.User.ClientId == serverRpc.Receive.SenderClientId);
        if (sessionUser == null)
        {
            NetworkNotify.Instance.PushClientRPC(
                "Can't find the user",
                1,
                NotificationStyleType.Error,
                targetClients: HELPERS.GetClient(serverRpc.Receive.SenderClientId));
            return;
        }

        var newTeam = _lobbyData.SessionTeams.FirstOrDefault(x => x.Id == teamId);
        if (newTeam == null)
        {
            NetworkNotify.Instance.PushClientRPC(
                "Can't find the team",
                1,
                NotificationStyleType.Error,
                targetClients: HELPERS.GetClient(serverRpc.Receive.SenderClientId));
            return;
        }

        if (sessionUser.Team == null)
            return;

        if (newTeam.Id == sessionUser.Team.Id)
        {
            NetworkNotify.Instance.PushClientRPC(
                "You already in this team",
                1,
                NotificationStyleType.Error,
                targetClients: HELPERS.GetClient(serverRpc.Receive.SenderClientId));
            return;
        }

        if (newTeam.CanJoin == false)
        {
            NetworkNotify.Instance.PushClientRPC(
                "Team is full",
                1,
                NotificationStyleType.Error,
                targetClients: HELPERS.GetClient(serverRpc.Receive.SenderClientId));
            return;
        }

        ChangeTeam(sessionUser, newTeam);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetReadyStateServerRpc(ServerRpcParams serverRpc = default)
    {
        if (_lobbyData.State != LobbyState.Waiting)
            return;

        var sessionUser = _lobbyData.SessionUsers.FirstOrDefault(x => x.User.ClientId == serverRpc.Receive.SenderClientId);
        if (sessionUser.IsValid == false)
            return;

        if (sessionUser.Team.Id == CONSTANTS.DEFAULT_TEAM_ID)
            return;

        if (sessionUser.State == UserState.Authenticated)
        {
            sessionUser.State = UserState.ReadyToStart;
        }
        else if (sessionUser.State == UserState.ReadyToStart)
        {
            sessionUser.State = UserState.Authenticated;
        }

        var isReady = sessionUser.State == UserState.ReadyToStart;
        UserReadyStateChanging?.Invoke(sessionUser.User.ClientId, isReady);
    }
}