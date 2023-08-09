using MadeNPlayShared;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamesContainer : MonoBehaviour
{
    [SerializeField] private Transform _contentContainer;
    [SerializeField] private GameView _gameViewTemplate;
    [SerializeField] private GamesNotFoundView _gamesNotFoundView;

    private readonly List<GameInfo> _games = new List<GameInfo>();
    private GameView _selectedGameView;
    private ColorTheme _currentTheme;

    public GameInfo SelectedGame => _games.FirstOrDefault(x => x.View == _selectedGameView);

    private void Start()
    {
        _currentTheme = ThemeManager.CurrentTheme;
    }

    public void AddGames(List<LocalGameData> gamesData)
    {
        foreach (var data in gamesData)
        {
            var view = Instantiate(_gameViewTemplate, _contentContainer);
            view.Init(
                _currentTheme.CreateMenuTheme.GameLineDeselected,
                _currentTheme.CreateMenuTheme.GameLineSelected,
                _currentTheme.CreateMenuTheme.GameLineHovered
                );
            view.Render(data);
            view.Selecting += OnViewSelecting;

            var game = new GameInfo()
            {
                Data = data,
                View = view
            };
            _games.Add(game);
        }
    }

    public void Clear()
    {
        _selectedGameView = null;

        foreach (var gameInfo in _games)
        {
            GameObject.Destroy(gameInfo.View.gameObject);
        }

        _gamesNotFoundView.gameObject.SetActive(false);
        _games.Clear();
    }

    public void ShowNotFoundMessage()
    {
        _gamesNotFoundView.gameObject.SetActive(true);
    }

    private void OnViewSelecting(GameView view)
    {
        if (view == null)
            return;

        if (_selectedGameView == view)
            return;

        if (_selectedGameView != null)
            _selectedGameView.Deselect();

        _selectedGameView = view;
        _selectedGameView.Select();
    }
}