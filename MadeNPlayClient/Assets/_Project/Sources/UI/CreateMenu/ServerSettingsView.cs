using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerSettingsView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _serverNameField;
    [SerializeField] private TMP_InputField _connectionPortField;
    [SerializeField] private Button _createButton;
    [SerializeField] private Button _cancelButton;

    private const string SERVER_DEFAULT_NAME = "My Server";
    private const ushort SERVER_DEFAULT_PORT = 23465;

    public event Action<ServerSettings> ServerSettingCreated;
    public event Action ServerCreatingCanceled;

    private void OnEnable()
    {
        _createButton.onClick.AddListener(CreateServer);
        _cancelButton.onClick.AddListener(Cancel);
    }

    private void OnDisable()
    {
        _createButton.onClick.RemoveListener(CreateServer);
        _cancelButton.onClick.RemoveListener(Cancel);
    }

    public void Show()
    {
        ResetData();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Cancel()
    {
        ServerCreatingCanceled?.Invoke();
    }

    private void CreateServer()
    {
        var connectionPort = ushort.Parse(_connectionPortField.text);
        var settings = new ServerSettings()
        {
            Port = connectionPort,
            ServerName = _serverNameField.text,
        };
        ServerSettingCreated?.Invoke(settings);
    }

    private void ResetData()
    {
        _serverNameField.text = SERVER_DEFAULT_NAME;
        _connectionPortField.text = SERVER_DEFAULT_PORT.ToString();
    }
}