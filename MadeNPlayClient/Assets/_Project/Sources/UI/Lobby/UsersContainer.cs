using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UsersContainer : MonoBehaviour
{
    [SerializeField] private UserView _userTemplate;

    private readonly List<UserView> _users = new List<UserView>();

    public IReadOnlyList<UserView> Users => _users;

    public void AddUser(ulong clientId, string userName, bool isCurrentUser, bool isHost)
    {
        var slot = Instantiate(_userTemplate, transform);
        slot.Init(ThemeManager.CurrentTheme.LobbyTheme.UserViewColors);
        slot.Render(clientId, userName, isCurrentUser, isHost);
        _users.Add(slot);
    }

    public void ChangeUserReadyState(ulong clientId, bool isReady)
    {
        var slot = _users.FirstOrDefault(x => x.ClientId == clientId);
        slot.RenderReadyStatus(isReady);
    }

    public void RemoveUser(ulong clientId)
    {
        var slot = _users.FirstOrDefault(x => x.ClientId == clientId);
        _users.Remove(slot);
        Destroy(slot.gameObject);
    }
}