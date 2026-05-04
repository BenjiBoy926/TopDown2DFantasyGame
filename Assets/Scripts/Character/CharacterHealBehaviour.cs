using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterHealBehaviour : MonoBehaviour
{
    [SerializeField] private float _moveToSpiralStartDuration = .5f;
    [SerializeField] private Ease _moveToSpiralStartEase = Ease.OutQuad;

    [Space]
    [SerializeField] private float _spiralRadius = 0.49f;
    [SerializeField] private int _spiralTurns = 3;
    [SerializeField] private float _spiralTurnDuration = 0.15f;

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
        Vector2 cellCenter = _character.CellToWorld(_character.CurrentCell);
        Vector2 spiralStart = cellCenter + Vector2.up * _character.CellSize * _spiralRadius;
        _character.SetIsRunning(true);
        yield return transform.DOMove(spiralStart, _moveToSpiralStartDuration)
            .SetEase(_moveToSpiralStartEase)
            .WaitForCompletion();
        yield return SpiralSequence();
        _character.SetIsRunning(false);
    }

    private IEnumerator SpiralSequence()
    {
        Vector2 circleCenter = _character.CellToWorld(_character.CurrentCell);
        float startTime = Time.time;
        float elapsedTime = Time.time - startTime;
        float totalTime = _spiralTurnDuration * _spiralTurns;

        while (elapsedTime < totalTime)
        {
            float percentComplete = elapsedTime / totalTime;
            float turnsCompleted = percentComplete * _spiralTurns;
            float currentTurnPercent = Mathf.Repeat(turnsCompleted, 1);
            float angle = 2 * Mathf.PI * currentTurnPercent;
            float x = Mathf.Cos(angle) * _character.CellWidth * _spiralRadius;
            float y = Mathf.Sin(angle) * _character.CellHeight * _spiralRadius;
            Vector2 offset = new(x, y);
            _character.Position = circleCenter + offset;

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        Vector2 cellCenter = _character.CellToWorld(_character.CurrentCell);
        _character.Position = cellCenter;
        yield break;
    }
}