using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class JoinMenuTheme
{
    [Header("COLUMS NAME")]
    public Color ColumnText;
    [Space(10)]
    public ColorBlock Column;

    [Space(20)]
    [Header("SERVERS CONTAINER")]
    public Color ServersLineSelected;
    public Color ServersLineHovered;
    public Color ServersLineDeselected;
    [Space(10)]
    public Color ServersTextSelected;
    public Color ServersTextHovered;
    public Color ServersTextDeselected;
}