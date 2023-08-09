using System;
using UnityEngine.EventSystems;

public class IconButton : IconWithBackplate, IInteractable
{
    public event Action Click;
    public event Action Hover;
    public event Action Unhover;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        Click?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Hover?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Unhover?.Invoke();
    }
}