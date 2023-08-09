using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateThemeInstaller : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollBar;
    [SerializeField] private HorizontalLayoutGroup _botControlContainer;
    [SerializeField] private HorizontalLayoutGroup _serverSettingsControlsContainer;
    [SerializeField] private Image _serverSettingsBackplate;

    public void SetTheme(CreateMenuTheme theme, CommonTheme commonTheme)
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

        var serverSettingsControls = _serverSettingsControlsContainer.GetComponentsInChildren<Button>(true);

        foreach (var control in serverSettingsControls)
        {
            control.colors = commonTheme.Controls.Controls;
            var text = control.GetComponentInChildren<TMP_Text>();
            text.color = commonTheme.Controls.ControlsTextDeselected;
        }

        _scrollBar.colors = commonTheme.Scrollbar;
        _serverSettingsBackplate.color = theme.ServerSettingBackplate;
    }
}