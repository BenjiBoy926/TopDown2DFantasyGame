using DG.Tweening;
using UnityEngine;

public class CharacterHealthUI : MonoBehaviour
{
    [SerializeField] private float _shakeDuration = 0.3f;
    [SerializeField] private float _shakeStrength = 0.5f;
    [SerializeField] private int _shakeVibrato = 100;
    [SerializeField] private float _shakeRandomness = 90;
    [SerializeField] private bool _shakeFadeOut = false;
    [SerializeField] private ShakeRandomnessMode _shakeRandomnessMode = ShakeRandomnessMode.Full;

    private CharacterStats _stats;
    private CharacterHealthbar _healthBar;
    private CharacterHeartIcon _heartIcon;

    public void ShowCurrentHealth()
    {
        _healthBar.ShowCurrentHealth();

        int currentHealth = _stats.CurrentHealth;
        int baseHealth = _stats.BaseHealth;
        float healthPercentage = (float)currentHealth / baseHealth;
        _heartIcon.ShowHealthPercent(healthPercentage);
    }

    public void Shake()
    {
        transform.DOShakePosition(
            _shakeDuration,
            _shakeStrength,
            _shakeVibrato,
            _shakeRandomness,
            false,
            _shakeFadeOut,
            _shakeRandomnessMode);
    }

    private void Awake()
    {
        _stats = GetComponentInParent<CharacterStats>();
        _healthBar = GetComponentInChildren<CharacterHealthbar>();
        _heartIcon = GetComponentInChildren<CharacterHeartIcon>();
    }
}