using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAttackBehaviour : MonoBehaviour
{
    private Character _character;

    public IEnumerator GetSequence(Character other)
    {
        _character.SecureCurrentCell();

        Vector2 thisPosition = _character.CellToWorld(_character.CurrentCell);
        Vector2 otherPosition = other.Position;
        _character.SetDirection(otherPosition - thisPosition);

        yield return _character.PlayAttackAnimation();
        yield return _character.WaitInCurrentCell();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}