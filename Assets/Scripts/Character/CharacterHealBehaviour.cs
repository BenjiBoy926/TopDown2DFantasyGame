using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterHealBehaviour : MonoBehaviour
{
    private Character _character;

    public IEnumerator GetSequence(Character other)
    {
        // heal them
        Debug.Log("Heal!");
        _character.SecureCurrentCell();

        Vector2 targetPosition = _character.CellToWorld(_character.CurrentCell);
        _character.LookAt(targetPosition);

        _character.SetIsRunning(true);
        yield return transform.DOMove(targetPosition, .3f)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();
        _character.SetIsRunning(false);

        yield return _character.MoveFadeOut();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}