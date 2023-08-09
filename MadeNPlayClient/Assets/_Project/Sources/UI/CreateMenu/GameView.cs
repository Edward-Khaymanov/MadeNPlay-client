using MadeNPlayShared;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameView : MonoBehaviour, IInteractable
{
    [SerializeField] private Image _backplate;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _id;
    [SerializeField] private TMP_Text _version;
    [SerializeField] private TMP_Text _teams;
    [SerializeField] private TMP_Text _players;
    [SerializeField] private TMP_Text _path;

    private Color _defaultColor;
    private Color _selectColor;
    private Color _hoverColor;
    private bool _selected;

    public event Action<GameView> Selecting;

    public void Init(Color defaultColor, Color selectColor, Color hoverColor)
    {
        _defaultColor = defaultColor;
        _selectColor = selectColor;
        _hoverColor = hoverColor;
        _backplate.color = defaultColor;
    }

    public void Render(LocalGameData data)
    {
        _icon.sprite = data.Icon;
        _name.text = $"{data.Name}";
        _id.text = $"ID: {data.Id}";
        _version.text = $"Version: {data.Version}";
        _teams.text = $"Teams: {data.Teams.Count}";
        _players.text = $"Players: {data.MaxPlayers}";
        _path.text = $"Path: {data.Path}";
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
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (_selected == false)
            Selecting?.Invoke(this);
    }
}