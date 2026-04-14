using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Character))]
public class CharacterCancelBehaviour : MonoBehaviour
{
    private Character _character;

    public IEnumerator GetSequence()
    {
        Vector2 targetPosition = _character.CellToWorld(_character.HomeCell);
        _character.LookAt(targetPosition);
        yield return _character.GetRunToSequence(targetPosition);
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}