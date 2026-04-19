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
    [SerializeField] private float _attackConnectPause = 1;

    [Space]
    [SerializeField] private float _fallbackDuration = .2f;
    [SerializeField] private Ease _fallbackEase = Ease.OutBack;

    private static readonly WaitForSeconds _dealDamageWait = new(.1f);
    private Character _character;

    public IEnumerator GetSequence(Character other)
    {
        _character.SecureCurrentCell();
        _character.LookAt(other.Position);

        Vector2 cellPosition = _character.CellToWorld(_character.CurrentCell);
        Vector2 towardsTarget = (other.Position - cellPosition).normalized;
        Vector2 chargeOffset = towardsTarget * _character.CellSize * 0.7f;
        Vector2 chargePosition = cellPosition + chargeOffset;

        _character.PlayAttackAnimation();
        yield return _character.transform.DOMove(cellPosition, _moveToCellCenterDuration)
            .SetEase(_moveToCellCenterEase)
            .WaitForCompletion();
        yield return _character.transform.DOMove(chargePosition, _chargeDuration)
            .SetEase(_chargeEase)
            .WaitForCompletion();

        WaitForSeconds pauseWait = new(_attackConnectPause);
        _character.PauseAnimation();
        yield return pauseWait;
        _character.ResumeAnimation();

        Coroutine otherFlinchRoutine = StartCoroutine(OtherFlinchSequence(other));
        yield return _character.transform.DOMove(cellPosition, _fallbackDuration)
            .SetEase(_fallbackEase)
            .WaitForCompletion();

        yield return otherFlinchRoutine;
        yield return _character.MoveFadeOut();
    }

    private IEnumerator OtherFlinchSequence(Character other)
    {
        other.LookAt(_character.Position);
        yield return _dealDamageWait;

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