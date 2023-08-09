using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainThemeInstaller : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup _controlsContainer;

    public void SetTheme(MainMenuTheme mainMenuTheme, CommonTheme commonTheme)
    {
        var controls = _controlsContainer.GetComponentsInChildren<ButtonWithText>(true);

        foreach (var control in controls)
        {
            control.IconColors = mainMenuTheme.Controls.Icon;
            control.TextColors = mainMenuTheme.Controls.Text;
        }
    }
}
