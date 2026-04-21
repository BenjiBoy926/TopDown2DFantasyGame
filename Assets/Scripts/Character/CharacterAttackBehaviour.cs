using DG.Tweening;
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
    [SerializeField] private float _attackConnectPause = 1;
    [SerializeField] private float _attackConnectShakeStrength = 0.1f;
    [SerializeField] private int _attackConnectShakeVibrato = 10;
    [SerializeField] private float _attackConnectShakeRandomness = 90;
    [SerializeField] private bool _attackConnectShakeFadeOut = true;
    [SerializeField] private ShakeRandomnessMode _attackConnectShakeRandomnessMode = ShakeRandomnessMode.Full;

    [Space]
    [SerializeField] private float _fallbackDuration = .2f;
    [SerializeField] private Ease _fallbackEase = Ease.OutBack;

    private Character _character;

    public IEnumerator GetSequence(Character other)
    {
        _character.SecureCurrentCell();
        _character.LookAt(other.Position);
        other.LookAt(_character.Position);

        Coroutine attackAnimation = _character.PlayAttackAnimation();
        yield return ChargeSequence(other);
        yield return AttackConnectSequence(other);
        yield return FallbackSequence(other, attackAnimation);
        yield return _character.MoveFadeOut();
    }

    private IEnumerator ChargeSequence(Character other)
    {
        Vector2 cellPosition = _character.CellToWorld(_character.CurrentCell);
        Vector2 towardsTarget = (other.Position - cellPosition).normalized;
        Vector2 chargeOffset = towardsTarget * _character.CellSize * 0.49f;
        Vector2 chargePosition = cellPosition + chargeOffset;

        yield return _character.transform.DOMove(cellPosition, _moveToCellCenterDuration)
            .SetEase(_moveToCellCenterEase)
            .WaitForCompletion();
        yield return _character.transform.DOMove(chargePosition, _chargeDuration)
            .SetEase(_chargeEase)
            .WaitForCompletion();
    }

    private IEnumerator AttackConnectSequence(Character other)
    {
        _character.PauseAnimation();
        yield return other.transform.DOShakePosition(
            _attackConnectPause, 
            _attackConnectShakeStrength, 
            _attackConnectShakeVibrato, 
            _attackConnectShakeRandomness, 
            false, 
            _attackConnectShakeFadeOut, 
            _attackConnectShakeRandomnessMode).WaitForCompletion();
        _character.ResumeAnimation();
    }

    private IEnumerator FallbackSequence(Character other, Coroutine attackAnimation)
    {
        Coroutine otherFlinchRoutine = StartCoroutine(OtherFlinchSequence(other));

        Vector2 cellPosition = _character.CellToWorld(_character.CurrentCell);
        yield return _character.transform.DOMove(cellPosition, _fallbackDuration)
            .SetEase(_fallbackEase)
            .WaitForCompletion();
        yield return attackAnimation;
        _character.PlayIdleAnimation();

        yield return otherFlinchRoutine;
    }

    private IEnumerator OtherFlinchSequence(Character other)
    {
        other.TakeDamageFrom(_character);
        yield return other.PlayHurtAnimation();
        if (other.IsDead)
        {
            yield return other.PlayDieAnimation();
            other.gameObject.SetActive(false);
        }
        else
        {
            other.PlayIdleAnimation();
        }
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}