using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterHealBehaviour : MonoBehaviour
{
    private Character _character;

    public IEnumerator GetSequence(Character other)
    {
        // heal them
        yield break;
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}