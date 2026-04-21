using DG.Tweening;
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
    [SerializeField] private float _recoilDuration = 0.1f;
    [SerializeField] private Ease _recoilOutEase = Ease.OutQuad;
    [SerializeField] private Ease _recoilInEase = Ease.InQuad;

    [Space]
    [SerializeField] private float _afterDeathWaitDuration = 1;
    [SerializeField] private float _floatAwayDuration = 0.5f;
    [SerializeField] private Ease _floatAwayEase = Ease.OutQuint;

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
            gameObject.SetActive(false);
        }
        else
        {
            _character.PlayIdleAnimation();
        }
    }

    private IEnumerator GetRecoilSequence(Character attacker)
    {
        Vector2 recoilDirection = (_character.Position - attacker.Position).normalized;
        Vector2 recoilOffset = recoilDirection * _character.CellSize * 0.49f;
        yield return transform.DOMove(recoilOffset, _recoilDuration)
            .SetRelative()
            .SetEase(_recoilOutEase)
            .WaitForCompletion();
        yield return transform.DOMove(-recoilOffset, _recoilDuration)
            .SetRelative()
            .SetEase(_recoilInEase)
            .WaitForCompletion();

        // To guarantee accuracy of the character's position after the recoil,
        // in case of any floating point errors during the tweening.
        _character.Position = _character.CellToWorld(_character.CurrentCell);
    }

    private IEnumerator GetDeathSequence()
    {
        yield return _character.PlayDieAnimation();
        yield return _afterDeathWait;

        // TODO: add a sprite fade out too
        float floatOffset = _character.CellHeight * .49f;
        yield return transform.DOMoveY(floatOffset, _floatAwayDuration)
            .SetRelative()
            .SetEase(_floatAwayEase)
            .WaitForCompletion();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
        _afterDeathWait = new(_afterDeathWaitDuration);
    }
}