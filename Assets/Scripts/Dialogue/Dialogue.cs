using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    private DialogueSpeechBubble _speechBubble;

    private void Awake()
    {
        _speechBubble = GetComponentInChildren<DialogueSpeechBubble>(true);
    }

    public IEnumerator Say(Character character, string text)
    {
        return _speechBubble.Say(character, text);
    }
}