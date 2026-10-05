using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterSpeech : MonoBehaviour
{
    private CharacterSpeechBubble _bubble;

    private void Awake()
    {
        _bubble = GetComponentInChildren<CharacterSpeechBubble>(true);
    }

    public IEnumerator Say(string text)
    {
        return _bubble.Say(text);
    }
}