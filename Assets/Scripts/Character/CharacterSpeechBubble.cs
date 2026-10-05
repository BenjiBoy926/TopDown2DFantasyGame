using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CharacterSpeechBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _body;
    private RectTransform _rectTransform;
    private Character _character;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _character = GetComponentInParent<Character>();
        gameObject.SetActive(false);
    }

    public IEnumerator Say(string text)
    {
        // TODO: move GetSpeechBubblePosition into this class
        Vector2 worldPos = _character.GetSpeechBubblePosition();
        Vector2 screenPos = _character.WorldToScreen(worldPos);
        _rectTransform.anchoredPosition = screenPos;

        gameObject.SetActive(true);
        _body.text = text;
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}