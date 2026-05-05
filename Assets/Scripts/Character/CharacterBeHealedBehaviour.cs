using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Character))]
public class CharacterBeHealedBehaviour : MonoBehaviour
{
    private Character _character;

    public IEnumerator GetSequence()
    {
        _character.RestoreHealth();
        yield break;
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}