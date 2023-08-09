using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CommonTheme
{
    [SerializeField] private CommonControlsColors _controlsColors;
    [Header("SCROLLBAR")]
    [SerializeField] private ColorBlock _scrollbar;

    public CommonControlsColors Controls => _controlsColors;
    public ColorBlock Scrollbar => _scrollbar;
}

[Serializable]
public class CommonControlsColors
{
    [field: SerializeField] public Color ControlsBackplate { get; private set; }
    [field: SerializeField] public Color ControlsTextSelected { get; private set; }
    [field: SerializeField] public Color ControlsTextDeselected { get; private set; }
    [field: SerializeField] public ColorBlock Controls { get; private set; }
}