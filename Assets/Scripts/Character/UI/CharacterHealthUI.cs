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
    private CharacterHealthText _healthText;
    private CharacterHeartIcon _heartIcon;

    public void ShowHealth()
    {
        int currentHealth = _stats.CurrentHealth;
        int baseHealth = _stats.BaseHealth;

        float healthPercentage = (float)currentHealth / baseHealth;
        _healthText.Refresh();
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
        _healthText = GetComponentInChildren<CharacterHealthText>();
        _heartIcon = GetComponentInChildren<CharacterHeartIcon>();
    }
}