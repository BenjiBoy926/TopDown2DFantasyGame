using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterSpeech : MonoBehaviour
{
    [SerializeField] private Transform _speechBubbleAnchor;
    private Character _character;
    private CharacterSpeechBubble _bubble;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _bubble = GetComponentInChildren<CharacterSpeechBubble>(true);
    }

    public Vector2 GetSpeechBubblePosition()
    {
        Vector2 localPosition = _speechBubbleAnchor.localPosition;
        if (_character.GetDirection().x < 0)
        {
            localPosition.x *= -1;
        }
        Vector2 worldPosition = _speechBubbleAnchor.parent.TransformPoint(localPosition);
        return worldPosition;
    }

    public IEnumerator Say(string text)
    {
        return _bubble.Say(text);
    }
}