using DG.Tweening;
using Hellmade.Sound;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAttackBehaviour : MonoBehaviour
{
    [SerializeField] private float _moveToCellCenterDuration = 0.01f;
    [SerializeField] private Ease _moveToCellCenterEase = Ease.Linear;

    [Space]
    [SerializeField] private float _chargeDuration = 0.09f;
    [SerializeField] private Ease _chargeEase = Ease.InBack;
    [SerializeField] private float _chargeDistance = 0.2f;

    [Space]
    [SerializeField] private AudioClip _windUpClip;
    [SerializeField] private AudioClip _attackConnectClip;
    [SerializeField] private AudioClip _hurtClip;

    private Character _character;
    private bool _isAttackInitiator = false;

    public IEnumerator GetSequence(Character other)
    {
        return _character.IsRanged || other.IsRanged ?
            GetAttackAloneSequence(other) :
            GetAttackExchangeSequence(other);
    }
    
    private IEnumerator GetAttackAloneSequence(Character other)
    {
        yield return BeginSequence(other);
        yield return GetMeleeSwipeSequence(other);

        EazySoundManager.PlaySound(_hurtClip);
        MoveToCellCenter();
        Coroutine otherHurtRoutine = StartCoroutine(other.GetHurtSequence(_character));
        while (_character.IsOneShotAnimationPlaying)
        {
            yield return null;
        }

        _character.PlayIdleAnimation();
        yield return otherHurtRoutine;

        yield return EndSequence(other);
        RemoveDeadCombatantsFromBattlefield(other);
    }

    private IEnumerator GetAttackExchangeSequence(Character other)
    {
        yield return BeginSequence(other);

        IEnumerator otherSwipeSequence = other.GetMeleeSwipeSequence(_character);
        Coroutine otherSwipeRoutine = StartCoroutine(otherSwipeSequence);
        yield return GetMeleeSwipeSequence(other);
        yield return otherSwipeRoutine;

        IEnumerator otherHurtSequence = other.GetHurtSequence(_character);
        Coroutine otherHurtRoutine = StartCoroutine(otherHurtSequence);
        yield return GetHurtSequence(other);
        yield return otherHurtRoutine;

        yield return EndSequence(other);
        RemoveDeadCombatantsFromBattlefield(other);
    }

    private IEnumerator BeginSequence(Character other)
    {
        _isAttackInitiator = true;

        _character.BeginMove();
        _character.LookAt(other.Position);
        other.LookAt(_character.Position);

        yield return MoveToCellCenter();
    }

    public IEnumerator GetMeleeSwipeSequence(Character other)
    {
        _character.PlayAttackAnimation();
        yield return ChargeSequence(other);
        yield return AttackConnectSequence(other);
    }

    private YieldInstruction MoveToCellCenter()
    {
        Vector2 cellPosition = _character.CurrentCellCenter;
        return transform.DOMove(cellPosition, _moveToCellCenterDuration)
            .SetEase(_moveToCellCenterEase)
            .WaitForCompletion();
    }

    private YieldInstruction ChargeSequence(Character other)
    {
        Vector2 cellPosition = _character.CurrentCellCenter;
        Vector2 towardsTarget = (other.Position - cellPosition).normalized;
        Vector2 chargeOffset = towardsTarget * _character.CellSize * _chargeDistance;
        Vector2 chargePosition = cellPosition + chargeOffset;

        if (_isAttackInitiator)
        {
            EazySoundManager.PlaySound(_windUpClip);
        }
        return transform.DOMove(chargePosition, _chargeDuration)
            .SetEase(_chargeEase)
            .WaitForCompletion();
    }

    private IEnumerator AttackConnectSequence(Character other)
    {
        if (_isAttackInitiator)
        {
            EazySoundManager.PlaySound(_attackConnectClip);
        }
        _character.PauseAnimation();
        yield return other.PlayAttackConnectShake();
        _character.ResumeAnimation();
    }

    private IEnumerator GetHurtSequence(Character other)
    {
        EazySoundManager.PlaySound(_hurtClip);
        return _character.GetHurtSequence(other);
    }

    private IEnumerator EndSequence(Character other)
    {
        _character.RecordMoveWith(other);
        bool shouldDisappear = _character.IsDead && !_character.CanBeRevived;
        if (!shouldDisappear)
        {
            yield return _character.PerformSpriteFade();
        }

        _isAttackInitiator = false;
        _character.EndMove();
    }

    private void RemoveDeadCombatantsFromBattlefield(Character other)
    {
        if (other.ShouldRemoveFromBattlefield())
        {
            other.gameObject.SetActive(false);
        }
        if (_character.ShouldRemoveFromBattlefield())
        {
            _character.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}