using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyThemeInstaller : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup _botControlContainer;

    public void SetTheme(LobbyTheme theme, CommonTheme commonTheme)
    {
        var botControls = _botControlContainer.GetComponentsInChildren<Button>(true);
        var botControlsBackplate = _botControlContainer.GetComponent<Image>();
        botControlsBackplate.color = commonTheme.Controls.ControlsBackplate;

        foreach (var control in botControls)
        {
            control.colors = commonTheme.Controls.Controls;
            var text = control.GetComponentInChildren<TMP_Text>();
            text.color = commonTheme.Controls.ControlsTextDeselected;
        }
    }
}