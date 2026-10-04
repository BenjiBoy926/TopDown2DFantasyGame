using UnityEngine;

public class CharacterSpeechBubble : MonoBehaviour
{
    private Character _character;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
    }

    public Coroutine Say(string text)
    {
        Debug.Log($"{_character.Name} says: {text}");
        return null;
    }
}