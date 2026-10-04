using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class DialogueSpeechBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _body;
    private RectTransform _rectTransform;
    private Battle _battle;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _battle = GetComponentInParent<Battle>();
        gameObject.SetActive(false);
    }

    public IEnumerator Say(Character character, string text)
    {
        Vector2 worldPos = character.GetSpeechBubblePosition();
        Vector2 screenPos = _battle.WorldToScreen(worldPos);
        _rectTransform.anchoredPosition = screenPos;

        gameObject.SetActive(true);
        _body.text = text;
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}