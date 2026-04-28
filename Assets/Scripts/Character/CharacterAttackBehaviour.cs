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

    [Space]
    [SerializeField] private float _fallbackDuration = .2f;
    [SerializeField] private Ease _fallbackEase = Ease.OutBack;

    [Space]
    [SerializeField] private AudioClip _attackConnectClip;

    private Character _character;

    public IEnumerator GetAttackSequence(Character other)
    {
        CharacterUI.HideAll();

        _character.SecureCurrentCell();
        _character.LookAt(other.Position);
        other.LookAt(_character.Position);

        Coroutine attackAnimation = _character.PlayAttackAnimation();
        yield return ChargeSequence(other);
        yield return AttackConnectSequence(other);

        Coroutine otherHurtRoutine = other.Hurt(_character);

        PlayFallbackTween();
        yield return attackAnimation;
        _character.PlayIdleAnimation();
        yield return otherHurtRoutine;

        yield return _character.MoveFadeOut();

        CharacterUI.ShowAll();
    }

    private IEnumerator ChargeSequence(Character other)
    {
        Vector2 cellPosition = _character.CellToWorld(_character.CurrentCell);
        Vector2 towardsTarget = (other.Position - cellPosition).normalized;
        Vector2 chargeOffset = towardsTarget * _character.CellSize * 0.49f;
        Vector2 chargePosition = cellPosition + chargeOffset;

        yield return transform.DOMove(cellPosition, _moveToCellCenterDuration)
            .SetEase(_moveToCellCenterEase)
            .WaitForCompletion();
        yield return transform.DOMove(chargePosition, _chargeDuration)
            .SetEase(_chargeEase)
            .WaitForCompletion();
    }

    private IEnumerator AttackConnectSequence(Character other)
    {
        EazySoundManager.PlaySound(_attackConnectClip);
        _character.PauseAnimation();
        yield return other.PlayAttackConnectShake();
        _character.ResumeAnimation();
    }

    private void PlayFallbackTween()
    {
        Vector2 cellPosition = _character.CellToWorld(_character.CurrentCell);
        transform.DOMove(cellPosition, _fallbackDuration).SetEase(_fallbackEase);
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}