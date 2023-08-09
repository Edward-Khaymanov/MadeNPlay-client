using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinThemeInstaller : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollBar;
    [SerializeField] private HorizontalLayoutGroup _columnsContainer;
    [SerializeField] private HorizontalLayoutGroup _botControlContainer;

    public void SetTheme(JoinMenuTheme theme, CommonTheme commonTheme)
    {
        var columns = _columnsContainer.GetComponentsInChildren<Button>(true);
        foreach (var column in columns)
        {
            column.colors = theme.Column;
            var text = column.GetComponentInChildren<TMP_Text>();
            text.color = theme.ColumnText;
        }

        var botControls = _botControlContainer.GetComponentsInChildren<Button>(true);
        var botControlsBackplate = _botControlContainer.GetComponent<Image>();
        botControlsBackplate.color = commonTheme.Controls.ControlsBackplate;

        foreach (var control in botControls)
        {
            control.colors = commonTheme.Controls.Controls;
            var text = control.GetComponentInChildren<TMP_Text>();
            text.color = commonTheme.Controls.ControlsTextDeselected;
        }

        _scrollBar.colors = commonTheme.Scrollbar;
    }
}