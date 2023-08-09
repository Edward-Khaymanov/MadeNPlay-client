using System;
using UnityEngine;

[Serializable]
public class CreateMenuTheme
{
    [Header("GAMES CONTAINER")]
    public Color GameLineSelected;
    public Color GameLineHovered;
    public Color GameLineDeselected;
    [Space(10)]
    public Color GameTextSelected;
    public Color GameTextHovered;
    public Color GameTextDeselected;

    [Space(20)]
    [Header("SERVER SETTINGS")]
    public Color ServerSettingBackplate;
}