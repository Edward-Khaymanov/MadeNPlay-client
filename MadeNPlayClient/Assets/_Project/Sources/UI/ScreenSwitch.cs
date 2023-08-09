using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSwitch : MonoBehaviour
{
    [SerializeField] private Image _wallpaper;
    [SerializeField] private MainMenuView _mainMenu;
    [SerializeField] private CreateMenuView _createMenu;
    [SerializeField] private JoinMenuView _joinMenu;

    private List<IScreen> _screens;
    
    public static ScreenSwitch Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _screens = new List<IScreen>()
        {
            _mainMenu,
            _createMenu,
            _joinMenu
        };
    }

    private void Start()
    {
        RegisterListeners();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            RemoveListeners();
            Instance = null;
        }
    }

    public void ShowMainMenu()
    {
        MakeBlur(0f);
        _mainMenu.Show();
        _createMenu.Hide();
        _joinMenu.Hide();
    }

    public void ShowCreateMenu()
    {
        MakeBlur(0.8f);
        _mainMenu.Hide();
        _createMenu.Show();
        _joinMenu.Hide();
    }

    public void ShowJoinMenu()
    {
        MakeBlur(0.8f);
        _mainMenu.Hide();
        _createMenu.Hide();
        _joinMenu.Show();
    }

    private void MakeBlur(float value)
    {
        _wallpaper.material.SetFloat("_Power", value);
    }

    private void RegisterListeners()
    {
        foreach (var screen in _screens)
        {
            screen.OnRegisterScreenListeners();
        }
    }

    private void RemoveListeners()
    {
        foreach (var screen in _screens)
        {
            screen.OnRemoveScreenListeners();
        }
    }
}