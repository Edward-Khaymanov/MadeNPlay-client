using NetcodeChat;
using MadeNPlayShared;
using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public partial class Lobby : NetworkBehaviour
{
    public event Action<ulong> ClientAuthenticated;
    public event Action<ulong> ClientDisconected;

    private void AuthenticateClient(ulong clientId)
    {
        var sessionUser = _lobbyData.SessionUsers.FirstOrDefault(x => x.User.ClientId == clientId);
        sessionUser.State = UserState.Authenticated;
        AddUserToTeam(sessionUser, _lobbyData.SessionTeams.FirstOrDefault(x => x.Id == CONSTANTS.DEFAULT_TEAM_ID));
        ClientAuthenticated?.Invoke(clientId);
    }

    private void OnClientConnected(ulong clientId)
    {
        AuthenticateClient(clientId);
    }

    private void OnClientDisconnected(ulong clientId)
    {
        var sessionUser = _lobbyData.SessionUsers.FirstOrDefault(x => x.User.ClientId == clientId);
        if (sessionUser == null)
            return;

        if (_lobbyData.State != LobbyState.Started && sessionUser.State != UserState.Connected)
            RemoveUserFromTeam(sessionUser);

        _lobbyData.SessionUsers.Remove(sessionUser);
        ClientDisconected?.Invoke(sessionUser.User.ClientId);
    }

    private void OnLoadedScene(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (sceneName == CONSTANTS.MENU_SCENE_NAME)
            return;

        var sessionUser = _lobbyData.SessionUsers.FirstOrDefault(x => x.User.ClientId == clientId);
        sessionUser.State = UserState.LoadedScene;
    }
}