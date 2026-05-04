using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterHealBehaviour : MonoBehaviour
{
    [SerializeField] private float _moveToCellCenterDuration = 0.35f;
    private Character _character;

    public IEnumerator GetSequence(Character other)
    {
        // heal them
        _character.SecureCurrentCell();

        _character.LookAt(other.Position);
        if (!other.IsDead)
        {
            other.LookAt(_character.Position);
        }

        Vector2 targetPosition = _character.CellToWorld(_character.CurrentCell);
        _character.SetIsRunning(true);
        yield return transform.DOMove(targetPosition, .3f)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();
        _character.SetIsRunning(false);

        yield return other.BeHealed();
        yield return _character.MoveFadeOut();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}