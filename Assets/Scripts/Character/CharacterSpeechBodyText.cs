using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(TMP_Text))]
public class CharacterSpeechBodyText : MonoBehaviour
{
    public Vector2 Size => _rectTransform.sizeDelta;

    [SerializeField] private float _maxWidth = 300f;
    private RectTransform _rectTransform;
    private TMP_Text _text;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _text = GetComponent<TMP_Text>();
    }

    public void Resize(string text)
    {
        Vector2 preferredSize = _text.GetPreferredValues(text);
        float multipleOfMax = Mathf.Ceil(preferredSize.x / _maxWidth);
        if (multipleOfMax > 1)
        {
            float containerWidth = preferredSize.x / multipleOfMax;
            preferredSize = _text.GetPreferredValues(text, containerWidth, float.MaxValue);
            preferredSize.x = containerWidth;
        }
        _rectTransform.sizeDelta = preferredSize;
    }

    public void Clear()
    {
        _text.text = "";
    }

    public void Append(char c)
    {
        _text.text += c;
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
}