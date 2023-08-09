using UnityEngine.EventSystems;

public interface IInteractable : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

}

public enum InteractableState
{
    Normal,
    Hovered,
    Pressed
}