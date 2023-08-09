using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    [SerializeField] private ColorTheme _theme;

    private MainThemeInstaller _mainThemeInstaller;
    private CreateThemeInstaller _createThemeInstaller;
    private JoinThemeInstaller _joinThemeInstaller;
    private LobbyThemeInstaller _lobbyThemeInstaller;

    public static ThemeManager Instance { get; private set; }
    public static ColorTheme CurrentTheme => Instance.Theme;
    public ColorTheme Theme => _theme;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SetThemes();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    
    private void SetThemes()
    {
        _mainThemeInstaller = GetComponentInChildren<MainThemeInstaller>();
        _mainThemeInstaller.SetTheme(_theme.MainMenuTheme, _theme.CommonTheme);

        _joinThemeInstaller = GetComponentInChildren<JoinThemeInstaller>();
        _joinThemeInstaller.SetTheme(_theme.JoinMenuTheme, _theme.CommonTheme);

        _createThemeInstaller = GetComponentInChildren<CreateThemeInstaller>();
        _createThemeInstaller.SetTheme(_theme.CreateMenuTheme, _theme.CommonTheme);

        _lobbyThemeInstaller = GetComponentInChildren<LobbyThemeInstaller>();
        _lobbyThemeInstaller.SetTheme(_theme.LobbyTheme, _theme.CommonTheme);
    }
}