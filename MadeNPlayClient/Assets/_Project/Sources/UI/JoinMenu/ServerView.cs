using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ServerView : MonoBehaviour, IInteractable
{
    [SerializeField] private Image _backplate;
    [SerializeField] private TMP_Text _serverName;
    [SerializeField] private TMP_Text _gameName;
    [SerializeField] private TMP_Text _version;
    [SerializeField] private TMP_Text _players;
    [SerializeField] private TMP_Text _ping;

    private Color _defaultColor;
    private Color _selectColor;
    private Color _hoverColor;
    private bool _selected;

    public event Action<ServerView> Selecting;

    public void Init(Color defaultColor, Color selectColor, Color hoverColor)
    {
        _defaultColor = defaultColor;
        _selectColor = selectColor;
        _hoverColor = hoverColor;
        _backplate.color = defaultColor;
    }

    public void Render(NetworkLobbyData data, int ping)
    {
        _serverName.text = data.ServerName;
        _gameName.text = data.NetworkGameData.Name;
        _version.text = data.NetworkGameData.Version;
        _players.text = $"{data.ActivePlayers}/{data.MaxPlayers}";
        _ping.text = $"{ping}";
    }

    public void Select()
    {
        _selected = true;
        _backplate.color = _selectColor;
    }

    public void Deselect()
    {
        _selected = false;
        _backplate.color = _defaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_selected == false)
            _backplate.color = _hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_selected == false)
            _backplate.color = _defaultColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_selected == false)
            Selecting?.Invoke(this);
    }
}