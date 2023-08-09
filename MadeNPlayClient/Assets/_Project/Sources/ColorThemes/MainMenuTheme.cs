using System;
using UnityEngine;

[Serializable]
public class MainMenuTheme
{
    [SerializeField] private MainControlsColors _controls;

    public MainControlsColors Controls => _controls;
}

[Serializable]
public class MainControlsColors
{
    [field: SerializeField] public InteractableColors Icon { get; private set; }
    [field: SerializeField] public InteractableColors Text { get; private set; }
}