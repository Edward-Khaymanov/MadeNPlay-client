using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonWithText : MonoBehaviour, IInteractable
{
    public Image IconSource;
    public TMP_Text TextSource;
    public InteractableColors IconColors;
    public InteractableColors TextColors;

    public event Action Click;
    public event Action Hover;
    public event Action Unhover;

    private void Start()
    {
        ChangeColors(IconColors.Normal, TextColors.Normal);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        Click?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeColors(IconColors.Highlighted, TextColors.Highlighted);
        Hover?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeColors(IconColors.Normal, TextColors.Normal);
        Unhover?.Invoke();
    }

    private void ChangeColors(Color icon, Color text)
    {
        IconSource.color = icon;
        TextSource.color = text;
    }
}