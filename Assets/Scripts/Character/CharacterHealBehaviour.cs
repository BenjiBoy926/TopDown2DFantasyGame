using DG.Tweening;
using Hellmade.Sound;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterHealBehaviour : MonoBehaviour
{
    [SerializeField] private float _moveToSpiralStartDuration = .5f;
    [SerializeField] private Ease _moveToSpiralStartEase = Ease.OutQuad;

    [Space]
    [SerializeField] private float _spiralPhi = Mathf.PI / 2;
    [SerializeField] private float _spiralRadius = 0.49f;
    [SerializeField] private int _spiralTurns = 3;
    [SerializeField] private float _spiralTurnDuration = 0.15f;

    [Space]
    [SerializeField] private AudioClip _healSound;

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

        EazySoundManager.PlaySound(_healSound);
        yield return DanceSequence();
        yield return other.BeHealed(_character);

        _character.RecordMoveWith(other);
        yield return _character.MoveFadeOut();
        CharacterUI.ShowAll();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private IEnumerator DanceSequence()
    {
        Vector2 spiralStart = GetSpiralPosition(0);

        _character.SetIsRunning(true);

        yield return transform.DOMove(spiralStart, _moveToSpiralStartDuration)
            .SetEase(_moveToSpiralStartEase)
            .WaitForCompletion();
        yield return SpiralSequence();
        yield return transform.DOMove(_character.CurrentCellCenter, _moveToSpiralStartDuration)
            .SetEase(_moveToSpiralStartEase)
            .WaitForCompletion();

        _character.SetIsRunning(false);
    }

    private IEnumerator SpiralSequence()
    {
        float startTime = Time.time;
        float elapsedTime = Time.time - startTime;
        float totalTime = _spiralTurnDuration * _spiralTurns;

        while (elapsedTime < totalTime)
        {
            float percentComplete = elapsedTime / totalTime;
            float turnsCompleted = percentComplete * _spiralTurns;
            float currentTurnPercent = Mathf.Repeat(turnsCompleted, 1);
            
            _character.Position = GetSpiralPosition(currentTurnPercent);

            yield return null;

            elapsedTime = Time.time - startTime;
        }
    }

    private Vector2 GetSpiralPosition(float currentTurnPercent)
    {
        return _character.CurrentCellCenter + GetSpiralOffset(currentTurnPercent);
    }

    private Vector2 GetSpiralOffset(float currentTurnPercent)
    {
        float angle = 2 * Mathf.PI * currentTurnPercent;
        float x = Mathf.Cos(angle + _spiralPhi) * _character.CellWidth * _spiralRadius;
        float y = Mathf.Sin(angle + _spiralPhi) * _character.CellHeight * _spiralRadius;
        return new(x, y);
    }
}