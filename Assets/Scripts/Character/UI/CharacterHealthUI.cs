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
    private CharacterHealthbarPreview _healthBarPreview;
    private CharacterHeartIcon _heartIcon;

    private void Awake()
    {
        _stats = GetComponentInParent<CharacterStats>();
        _healthBar = GetComponentInChildren<CharacterHealthbar>();
        _healthBarPreview = GetComponentInChildren<CharacterHealthbarPreview>(true);
        _heartIcon = GetComponentInChildren<CharacterHeartIcon>();
    }

    public void Preview(int health)
    {
        int higher = Mathf.Max(health, _stats.CurrentHealth);
        int lower = Mathf.Min(health, _stats.CurrentHealth);
        _healthBar.ShowHealth(lower);
        ShowHealthOnHeartIcon(health);
        _healthBarPreview.Show();
        _healthBarPreview.SetFill(higher);
    }

    public void ClearPreview()
    {
        _healthBarPreview.Hide();
        ShowCurrentHealth();
    }

    public void ShowCurrentHealth()
    {
        ShowHealth(_stats.CurrentHealth);
    }

    public void ShowHealth(int health)
    {
        _healthBar.ShowHealth(health);
        ShowHealthOnHeartIcon(health);
    }

    public void ShowHealthOnHeartIcon(int health)
    {
        int baseHealth = _stats.BaseHealth;
        float healthPercentage = (float)health / baseHealth;
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
}