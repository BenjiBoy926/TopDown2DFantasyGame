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

    public Coroutine Say(string text)
    {
        StopAllCoroutines();
        return StartCoroutine(_bubble.GetDisplaySequence(text));
    }
}