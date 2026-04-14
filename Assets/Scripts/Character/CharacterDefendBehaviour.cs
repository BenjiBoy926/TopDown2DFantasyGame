using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Character))]
public class CharacterDefendBehaviour : MonoBehaviour
{
    private Character _character;

    public IEnumerator GetSequence()
    {
        _character.SecureCurrentCell();

        Vector2 targetPosition = _character.CellToWorld(_character.CurrentCell);
        _character.SetDirection(targetPosition - _character.Position);
        
        yield return _character.WaitInCurrentCell();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}