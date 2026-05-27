using DG.Tweening;
using TMPro;
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
    private CharacterHealthbar _healthbar;
    private CharacterHeartIcon _heartIcon;
    private TMP_Text _label;

    public void ShowHealth()
    {
        int currentHealth = _stats.CurrentHealth;
        int baseHealth = _stats.BaseHealth;

        float healthPercentage = (float)currentHealth / baseHealth;
        _healthbar.Refresh();
        _heartIcon.ShowHealthPercent(healthPercentage);

        if (_label)
        {
            _label.text = currentHealth.ToString();
        }
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
        _healthbar = GetComponentInChildren<CharacterHealthbar>();
        _label = GetComponentInChildren<TMP_Text>();
        _heartIcon = GetComponentInChildren<CharacterHeartIcon>();
    }
}