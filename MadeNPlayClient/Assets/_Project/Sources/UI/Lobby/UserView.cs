using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserView : MonoBehaviour
{
    [SerializeField] private Image _backplateSource;
    [SerializeField] private IconWithBackplate _readyIconSource;
    [SerializeField] private TMP_Text _userNameSource;
    [SerializeField] private Sprite _readyIcon;
    [SerializeField] private Sprite _notReadyIcon;
    [SerializeField] private Sprite _hostIcon;

    private UserViewColors _colors;
    private bool _isHost;

    public ulong ClientId { get; private set; }

    public void Init(UserViewColors colors)
    {
        _colors = colors;
    }

    public void Render(ulong clientId, string nickname, bool isCurrentUser, bool isHost)
    {
        var backplateColor = isCurrentUser ? _colors.CurrentUserBackplate : _colors.Backplate;
        ClientId = clientId;
        _isHost = isHost;
        _userNameSource.text = nickname;
        _backplateSource.color = backplateColor;
    }

    public void RenderReadyStatus(bool isReady)
    {
        if (_isHost)
        {
            _readyIconSource.Render(null, _hostIcon);
            _readyIconSource.RenderColors(_colors.HostIconBackplate, _colors.HostIcon);
            return;
        }

        if (isReady)
        {
            _readyIconSource.Render(null, _readyIcon);
            _readyIconSource.RenderColors(_colors.ReadyIconBackplateReady, _colors.ReadyIconReady);
        }
        else
        {
            _readyIconSource.Render(null, _notReadyIcon);
            _readyIconSource.RenderColors(_colors.ReadyIconBackplateNotReady, _colors.ReadyIconNotReady);
        }
    }
}