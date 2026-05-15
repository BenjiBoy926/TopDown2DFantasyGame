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

    public IEnumerator GetFullAttackSequence(Character other)
    {
        CharacterUI.HideAll();
        _isAttackInitiator = true;

        _character.SecureCurrentCell();
        _character.LookAt(other.Position);
        other.LookAt(_character.Position);

        yield return MoveToCellCenter();

        IEnumerator otherSwipeSequence = other.GetAttackSwipeSequence(_character);
        Coroutine otherSwipeRoutine = StartCoroutine(otherSwipeSequence);
        yield return GetAttackSwipeSequence(other);
        yield return otherSwipeRoutine;

        if (_isAttackInitiator)
        {
            EazySoundManager.PlaySound(_hurtClip);
        }

        IEnumerator otherHurtSequence = other.GetHurtSequence(_character);
        Coroutine otherHurtRoutine = StartCoroutine(otherHurtSequence);
        yield return _character.GetHurtSequence(other);
        yield return otherHurtRoutine;

        yield return _character.MoveFadeOut();

        CharacterUI.ShowAll();
        _isAttackInitiator = false;

        // CAUTION: if any other coroutine waits for this coroutine and the attacker dies,
        // that coroutine will be waiting indefinitely since the attacker's game object will be disabled
        // and unable to continue running coroutines. It is not CURRENTLY a bug because no other coroutine waits for this one,
        // but could become a bug in the future if this warning is not heeded!
        if (other.IsDead && !other.CanBeRevived)
        {
            other.gameObject.SetActive(false);
        }
        if (_character.IsDead && !_character.CanBeRevived)
        {
            _character.gameObject.SetActive(false);
        }
    }

    public IEnumerator GetAttackSwipeSequence(Character other)
    {
        _character.PlayAttackAnimation();
        yield return ChargeSequence(other);
        yield return AttackConnectSequence(other);
    }

    private IEnumerator MoveToCellCenter()
    {
        Vector2 cellPosition = _character.CurrentCellCenter;
        yield return transform.DOMove(cellPosition, _moveToCellCenterDuration)
            .SetEase(_moveToCellCenterEase)
            .WaitForCompletion();
    }

    private IEnumerator ChargeSequence(Character other)
    {
        Vector2 cellPosition = _character.CurrentCellCenter;
        Vector2 towardsTarget = (other.Position - cellPosition).normalized;
        Vector2 chargeOffset = towardsTarget * _character.CellSize * _chargeDistance;
        Vector2 chargePosition = cellPosition + chargeOffset;

        if (_isAttackInitiator)
        {
            EazySoundManager.PlaySound(_windUpClip);
        }
        yield return transform.DOMove(chargePosition, _chargeDuration)
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

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}