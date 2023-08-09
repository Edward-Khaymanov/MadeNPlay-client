using MadeNPlayShared;
using System.Text;
using UINotify;
using Unity.Netcode;
using UnityEngine;

public class Authenticator : MonoBehaviour
{
    private LobbyData _lobbyData;

    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnect;
    }

    public void Init(LobbyData lobbyData)
    {
        _lobbyData = lobbyData;
    }

    private void OnDisconnect(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
            return;

        var reason = NetworkManager.Singleton.DisconnectReason;
        if (string.IsNullOrEmpty(reason))
            return;

        LocalNotify.Show($"You have been kicked. Reason: {reason}", 3, NotificationStyleType.Error);
    }

    private void ApprovalCheck(
        NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {
        var reasonBuilder = new StringBuilder();
        var canJoin = true;
        var requestData = Encoding.ASCII.GetString(request.Payload);
        var authData = JsonUtility.FromJson<AuthData>(requestData);

        if (request.ClientNetworkId != NetworkManager.ServerClientId)
            canJoin = CanJoin(reasonBuilder);

        if (canJoin)
        {
            var netUser = new NetworkUser(request.ClientNetworkId, authData.UserName);
            var sessionUser = new SessionUser()
            {
                UserId = authData.UserId,
                User = netUser,
                State = UserState.Connected
            };

            _lobbyData.SessionUsers.Add(sessionUser);
        }

        response.Reason = reasonBuilder.ToString();
        response.Approved = canJoin;
        response.Pending = false;
    }

    public bool CanJoin(StringBuilder reasonBuilder)
    {
        switch (_lobbyData.State)
        {
            case LobbyState.Waiting:
                return HaveSlots(reasonBuilder);
            default:
                return false;
        }
    }

    private bool HaveSlots(StringBuilder reasonBuilder)
    {
        var result = _lobbyData.CurrentPlayers + 1 <= _lobbyData.MaxPlayers;
        if (result == false)
            reasonBuilder.AppendLine("Maximum players on the server");

        return result;
    }
}