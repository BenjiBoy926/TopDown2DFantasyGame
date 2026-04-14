using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAttackBehaviour : MonoBehaviour
{
    private Character _character;

    public IEnumerator GetSequence(Character other)
    {
        _character.SecureCurrentCell();
        _character.LookAt(other.Position);
        yield return _character.PlayAttackAnimation();
        yield return _character.WaitInCurrentCell();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}