using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(TMP_Text))]
public class CharacterSpeechBodyText : MonoBehaviour
{
    public Vector2 Size => _rectTransform.sizeDelta;

    private RectTransform _rectTransform;
    private TMP_Text _text;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _text = GetComponent<TMP_Text>();
    }

    public void SetText(string text)
    {
        Vector2 preferredSize = _text.GetPreferredValues(text);
        _rectTransform.sizeDelta = preferredSize;
        _text.text = text;
    }
}