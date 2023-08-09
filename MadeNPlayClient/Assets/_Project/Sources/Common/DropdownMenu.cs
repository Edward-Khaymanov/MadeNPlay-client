using UnityEngine;

public class DropdownMenu : MonoBehaviour
{
    [SerializeField] private IconButton _toggleButton;
    [SerializeField] private Transform _contentContainer;

    private bool _isExpanded => _contentContainer.gameObject.activeSelf;

    private void OnEnable()
    {
        _toggleButton.Click += ToggleMenu;
    }

    private void OnDisable()
    {
        _toggleButton.Click -= ToggleMenu;
    }

    public void ToggleMenu()
    {
        if (_isExpanded)
            Hide();
        else
            Show();
    }

    public void Show()
    {
        if (_isExpanded)
            return;

        _contentContainer.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (_isExpanded == false)
            return;

        _contentContainer.gameObject.SetActive(false);
    }
}