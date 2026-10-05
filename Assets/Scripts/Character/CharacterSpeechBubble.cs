using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CharacterSpeechBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _body;
    [SerializeField] private Transform _anchor;
    private RectTransform _rectTransform;
    private Character _character;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _character = GetComponentInParent<Character>();
        _name.text = _character.Name;
        gameObject.SetActive(false);
    }

    public IEnumerator Say(string text)
    {
        _rectTransform.anchoredPosition = GetTargetScreenPosition();

        gameObject.SetActive(true);
        _body.text = text;
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