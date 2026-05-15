using DG.Tweening;
using Hellmade.Sound;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterHurtBehaviour : MonoBehaviour
{
    [SerializeField] private float _shakeDuration = 0.3f;
    [SerializeField] private float _shakeStrength = 0.5f;
    [SerializeField] private int _shakeVibrato = 100;
    [SerializeField] private float _shakeRandomness = 90;
    [SerializeField] private bool _shakeFadeOut = false;
    [SerializeField] private ShakeRandomnessMode _shakeRandomnessMode = ShakeRandomnessMode.Full;

    [Space]
    [SerializeField] private float _returnToCellBeforeRecoilDuration = 0.05f;
    [SerializeField] private float _recoilDuration = 0.1f;
    [SerializeField] private float _recoilDistance = 0.49f;

    [Space]
    [SerializeField] private float _afterDeathWaitDuration = 1;
    [SerializeField] private float _fadeAwayDuration = 0.5f;
    [SerializeField] private Ease _fadeAwayEase = Ease.OutQuad;

    [Space]
    [SerializeField] private AudioClip _deathClip;

    private Character _character;
    private WaitForSeconds _afterDeathWait;

    public YieldInstruction PlayAttackConnectShake()
    {
        return transform.DOShakePosition(
            _shakeDuration,
            _shakeStrength,
            _shakeVibrato,
            _shakeRandomness,
            false,
            _shakeFadeOut,
            _shakeRandomnessMode).WaitForCompletion();
    }

    public IEnumerator GetHurtSequence(Character attacker)
    {
        _character.TakeDamageFrom(attacker);

        Coroutine hurtAnimation = _character.PlayHurtAnimation();
        yield return GetRecoilSequence(attacker);
        yield return hurtAnimation;

        if (_character.IsDead)
        {
            yield return GetDeathSequence();
            if (!_character.CanBeRevived)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            _character.PlayIdleAnimation();
        }
    }

    private IEnumerator GetRecoilSequence(Character attacker)
    {
        yield return transform.DOMove(_character.CurrentCellCenter, _returnToCellBeforeRecoilDuration).WaitForCompletion();

        Vector2 recoilDirection = (_character.Position - attacker.Position).normalized;
        Vector2 recoilOffset = recoilDirection * _character.CellSize * _recoilDistance;
        yield return transform.DOPunchPosition(recoilOffset, _recoilDuration, 0, 0)
            .WaitForCompletion();

        // To guarantee accuracy of the character's position after the recoil,
        // in case of any floating point errors during the tweening.
        _character.Position = _character.CurrentCellCenter;
    }

    private IEnumerator GetDeathSequence()
    {
        EazySoundManager.PlaySound(_deathClip);
        yield return _character.PlayDieAnimation();
        yield return _afterDeathWait;
        if (!_character.CanBeRevived)
        {
            yield return _character.FadeAlpha(0, _fadeAwayDuration, _fadeAwayEase);
        }
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
        _afterDeathWait = new(_afterDeathWaitDuration);
    }
}