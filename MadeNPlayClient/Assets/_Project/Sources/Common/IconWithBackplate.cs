using UnityEngine;
using UnityEngine.UI;

public class IconWithBackplate : MonoBehaviour
{
    [SerializeField] private Image _backplateSource;
    [SerializeField] private Image _iconSource;

    public void Render(Sprite backplate, Sprite icon)
    {
        _backplateSource.sprite = backplate;
        _iconSource.sprite = icon;
    }

    public void RenderColors(Color backplateColor, Color iconColor)
    {
        _backplateSource.color = backplateColor;
        _iconSource.color = iconColor;
    }
}