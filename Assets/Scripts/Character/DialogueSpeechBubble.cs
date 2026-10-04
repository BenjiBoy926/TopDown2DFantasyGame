using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueSpeechBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _body;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator Say(Character character, string text)
    {
        gameObject.SetActive(true);
        _body.text = text;
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}