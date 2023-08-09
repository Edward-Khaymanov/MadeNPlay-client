using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour, IScreen
{
    [SerializeField] private ButtonWithText _createButton;
    [SerializeField] private ButtonWithText _joinButton;
    [SerializeField] private ButtonWithText _settingButton;
    [SerializeField] private ButtonWithText _exitButton;

    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        ScreenSwitch.Instance.ShowMainMenu();
    }

    private void OnEnable()
    {
        _exitButton.Click += Exit;
    }

    private void OnDisable()
    {
        _exitButton.Click -= Exit;
    }

    public void Show()
    {
        _canvas.enabled = true;
    }

    public void Hide()
    {
        _canvas.enabled = false;
    }

    private void Exit()
    {
        Application.Quit();
    }

    public void OnRegisterScreenListeners()
    {
        _createButton.Click += ScreenSwitch.Instance.ShowCreateMenu;
        _joinButton.Click += ScreenSwitch.Instance.ShowJoinMenu;
    }

    public void OnRemoveScreenListeners()
    {
        _createButton.Click -= ScreenSwitch.Instance.ShowCreateMenu;
        _joinButton.Click -= ScreenSwitch.Instance.ShowJoinMenu;
    }
}