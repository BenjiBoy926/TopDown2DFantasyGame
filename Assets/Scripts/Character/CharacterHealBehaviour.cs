using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterHealBehaviour : MonoBehaviour
{
    private Character _character;

    public IEnumerator GetSequence(Character other)
    {
        CharacterUI.HideAll();
        _character.SecureCurrentCell();
        _character.LookAt(other.Position);
        if (!other.IsDead)
        {
            other.LookAt(_character.Position);
        }

        yield return DanceSequence();
        yield return other.BeHealed();
        yield return _character.MoveFadeOut();
        CharacterUI.ShowAll();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private IEnumerator DanceSequence()
    {
        Vector2 targetPosition = _character.CellToWorld(_character.CurrentCell);
        _character.SetIsRunning(true);
        yield return transform.DOMove(targetPosition, .3f)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();
        _character.SetIsRunning(false);
    }
}