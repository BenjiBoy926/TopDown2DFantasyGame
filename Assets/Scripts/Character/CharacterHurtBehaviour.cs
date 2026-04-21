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

    private Character _character;

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
        // TODO: recoil away with a tween
        yield return _character.PlayHurtAnimation();
        if (_character.IsDead)
        {
            yield return _character.PlayDieAnimation();
            gameObject.SetActive(false);
        }
        else
        {
            _character.PlayIdleAnimation();
        }
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}