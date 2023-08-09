using NetcodeChat;
using UINotify;
using UnityEngine;
using UnityEngine.UI;

public class CreateMenuView : MonoBehaviour, IScreen
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private ServerSettingsView _serverSettingsView;
    [SerializeField] private GamesContainer _gamesContainer;
    [SerializeField] private LobbyInitializer _lobbyInitializer;

    private CreateMenuPresenter _presenter;

    private void Awake()
    {
        var canvas = GetComponent<Canvas>();
        _presenter = new CreateMenuPresenter(
            canvas, 
            _selectButton,
            _gamesContainer, 
            _serverSettingsView,
            _lobbyInitializer);
    }

    public void Show()
    {
        _presenter.Show();
    }

    public void Hide()
    {
        _presenter.Hide();
    }

    public void OnRegisterScreenListeners()
    {
        _backButton.onClick.AddListener(ScreenSwitch.Instance.ShowMainMenu);
    }

    public void OnRemoveScreenListeners()
    {
        _backButton.onClick.RemoveListener(ScreenSwitch.Instance.ShowMainMenu);
    }
}