using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerInfoView : MonoBehaviour
{
    [SerializeField] private TMP_Text _serverName;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _id;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _version;

    public void Render(NetworkLobbyData data)
    {
        _serverName.text = $"{data.ServerName}";
        _id.text = $"ID: {data.NetworkGameData.Id}";
        _name.text = $"Name: {data.NetworkGameData.Name}";
        _version.text = $"Version: {data.NetworkGameData.Version}";
        RenderIcon(data.NetworkGameData.Id, data.NetworkGameData.Version);
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Clear()
    {
        Hide();
        _serverName.text = string.Empty;
        _id.text = string.Empty;
        _name.text = string.Empty;
        _version.text = string.Empty;
        _icon.sprite = null;
    }

    private void RenderIcon(Guid gameId, string gameVersion)
    {
        var gameProvider = new GameProvider(AppContext.Current.Enviroment.GamesFolderPath);
        var gamePath = gameProvider.GetGamePath(gameId, gameVersion);
        if (string.IsNullOrEmpty(gamePath))
            return;

        var iconPath = gameProvider.GetGameIconPath(gamePath);
        var icon = gameProvider.GetGameIcon(iconPath);
        _icon.sprite = icon;
    }
}