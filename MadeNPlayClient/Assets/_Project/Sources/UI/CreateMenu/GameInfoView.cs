using MadeNPlayShared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _teamsCount;

    public void Render(LocalGameData localGameData)
    {
        _icon.sprite = localGameData.Icon;
        _name.text = $"Name: {localGameData.Name}";
        _teamsCount.text = $"Teams count:  {localGameData.Teams.Count}";
        _icon.enabled = true;
    }

    public void Clear()
    {
        _icon.sprite = null;
        _name.text = string.Empty;
        _teamsCount.text = string.Empty;
        _icon.enabled = false;
    }
}