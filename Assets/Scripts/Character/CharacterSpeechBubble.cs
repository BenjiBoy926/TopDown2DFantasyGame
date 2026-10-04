using System.Collections;
using TMPro;
using UnityEngine;

public class CharacterSpeechBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _body;
    private Character _character;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        gameObject.SetActive(false);
    }

    public IEnumerator Say(string text)
    {
        gameObject.SetActive(true);
        _body.text = text;
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}