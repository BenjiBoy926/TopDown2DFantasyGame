using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CharacterSpeechBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Transform _anchor;
    [SerializeField] private Vector2 _textPadding;
    [SerializeField] private float _minWidth = 100;
    [SerializeField] private float _timeBetweenCharacters = 0.05f;
    private RectTransform _rectTransform;
    private CharacterSpeechBodyText _body;
    private Character _character;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _body = GetComponentInChildren<CharacterSpeechBodyText>();
        _character = GetComponentInParent<Character>();
        _name.text = _character.Name;
        gameObject.SetActive(false);
    }

    public IEnumerator Say(string text)
    {
        gameObject.SetActive(true);
        _rectTransform.anchoredPosition = GetTargetScreenPosition();

        AdjustSize(text);

        WaitForSeconds charWait = new(_timeBetweenCharacters);
        _body.Clear();
        for (int i = 0; i < text.Length; i++)
        {
            _body.Append(text[i]);
            yield return charWait;
        }

        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    private void AdjustSize(string text)
    {
        _body.AdjustSize(text);
        Vector2 size = _body.Size + _textPadding;
        size.x = Mathf.Max(size.x, _minWidth);
        _rectTransform.sizeDelta = size;
    }

    private Vector2 GetTargetScreenPosition()
    {
        Vector2 worldPos = GetTargetWorldPosition();
        return _character.WorldToScreen(worldPos);
    }

    private Vector2 GetTargetWorldPosition()
    {
        Vector2 localPosition = _anchor.localPosition;
        if (_character.GetDirection().x < 0)
        {
            localPosition.x *= -1;
        }
        Vector2 worldPosition = _anchor.parent.TransformPoint(localPosition);
        return worldPosition;
    }
}