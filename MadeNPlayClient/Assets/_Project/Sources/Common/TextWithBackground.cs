using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextWithBackground : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private TMP_Text _textComponent;

    public void RenderText(string text)
    {
        _textComponent.text = text;
    }

    public void ChangeTextColor(Color color)
    {
        _textComponent.color = color;
    }

    public void RenderBackground(Sprite sprite)
    {
        _background.sprite = sprite;
    }

    public void ChangeBackgroundColor(Color color)
    {
        _background.color = color;
    }
}