using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CharacterSpeechBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Transform _anchor;
    [SerializeField] private Vector2 _textPadding;
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
        _rectTransform.anchoredPosition = GetTargetScreenPosition();

        gameObject.SetActive(true);
        _body.SetText(text);
        _rectTransform.sizeDelta = _body.Size + _textPadding;
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
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