using UnityEngine;

[CreateAssetMenu(fileName = "ColorTheme", menuName = "New color theme")]
public class ColorTheme : ScriptableObject
{
    [SerializeField] private CommonTheme _commonTheme;
    [SerializeField] private MainMenuTheme _mainMenuTheme;
    [SerializeField] private CreateMenuTheme _createMenuTheme;
    [SerializeField] private JoinMenuTheme _joinMenuTheme;
    [SerializeField] private LobbyTheme _lobbyTheme;

    public CommonTheme CommonTheme => _commonTheme;
    public MainMenuTheme MainMenuTheme => _mainMenuTheme;
    public CreateMenuTheme CreateMenuTheme => _createMenuTheme;
    public JoinMenuTheme JoinMenuTheme => _joinMenuTheme;
    public LobbyTheme LobbyTheme => _lobbyTheme;
}