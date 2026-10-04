using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CharacterSpeech : MonoBehaviour
{
    [SerializeField] private float _timeBetweenCharacters = 0.05f;
    [SerializeField] private float _pauseAtPunctuation = 0.3f;
    private CharacterSpeechBubble _bubble;

    private void Awake()
    {
        _bubble = GetComponentInChildren<CharacterSpeechBubble>();
    }

    public Coroutine Say(string text)
    {
        return _bubble.Say(text);
    }
}