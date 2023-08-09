using UINotify;
using UnityEngine;
using UnityEngine.UI;

public class CreateMenuPresenter
{
    private readonly Canvas _canvas;
    private readonly Button _selectButton;
    private readonly GamesContainer _gamesContainer;
    private readonly ServerSettingsView _serverSettingsView;
    private readonly LobbyInitializer _lobbyInitializer;

    public CreateMenuPresenter(
        Canvas canvas,
        Button selectButton,
        GamesContainer gamesContainer,
        ServerSettingsView serverSettingsView,
        LobbyInitializer lobbyInitializer)
    {
        _canvas = canvas;
        _selectButton = selectButton;
        _gamesContainer = gamesContainer;
        _serverSettingsView = serverSettingsView;
        _lobbyInitializer = lobbyInitializer;
    }

    public void Show()
    {
        LoadGames();
        _selectButton.onClick.AddListener(StartCreatingServer);
        _serverSettingsView.ServerSettingCreated += CreateServer;
        _serverSettingsView.ServerCreatingCanceled += CancelCreatingServer;
        _canvas.enabled = true;
    }

    public void Hide()
    {
        _canvas.enabled = false;
        _serverSettingsView.Hide();
        _gamesContainer.Clear();
        _selectButton.onClick.RemoveListener(StartCreatingServer);
        _serverSettingsView.ServerSettingCreated -= CreateServer;
        _serverSettingsView.ServerCreatingCanceled -= CancelCreatingServer;
    }

    private void StartCreatingServer()
    {
        if (_gamesContainer.SelectedGame == null)
        {
            LocalNotify.Show("Select a game", 1, NotificationStyleType.Error);
            return;
        }

        _serverSettingsView.Show();
    }

    private void CancelCreatingServer()
    {
        _serverSettingsView.Hide();
    }

    private void CreateServer(ServerSettings settings)
    {
        var gameData = _gamesContainer.SelectedGame.Data;
        _lobbyInitializer.CreateLobby(settings, gameData);
        Hide();
    }

    private void LoadGames()
    {
        var gameProvider = new GameProvider(AppContext.Current.Enviroment.GamesFolderPath);
        var localGames = gameProvider.GetLocalGames();
        if (localGames.Count == 0)
            _gamesContainer.ShowNotFoundMessage();
        else
            _gamesContainer.AddGames(localGames);
    }
}