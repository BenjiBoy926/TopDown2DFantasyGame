using DG.Tweening;
using TMPro;
using UnityEngine;

public class CharacterHealthUI : MonoBehaviour
{
    [SerializeField] private Sprite _healthFullSprite;
    [SerializeField] private Sprite _healthDamagedSprite;
    [SerializeField] private Sprite _healthDepletedSprite;

    [Space]
    [SerializeField] private float _shakeDuration = 0.3f;
    [SerializeField] private float _shakeStrength = 0.5f;
    [SerializeField] private int _shakeVibrato = 100;
    [SerializeField] private float _shakeRandomness = 90;
    [SerializeField] private bool _shakeFadeOut = false;
    [SerializeField] private ShakeRandomnessMode _shakeRandomnessMode = ShakeRandomnessMode.Full;

    private CharacterStats _stats;
    private SpriteSlider _slider;
    private TMP_Text _label;
    private SpriteRenderer _renderer;

    public void ShowHealth()
    {
        int currentHealth = _stats.CurrentHealth;
        int baseHealth = _stats.BaseHealth;

        float healthPercentage = (float)currentHealth / baseHealth;
        _slider.Value = healthPercentage;

        if (_label)
        {
            _label.text = currentHealth.ToString();
        }

        if (currentHealth >= baseHealth)
        {
            _renderer.sprite = _healthFullSprite;
        }
        else if (currentHealth > 0)
        {
            _renderer.sprite = _healthDamagedSprite;
        }
        else
        {
            _renderer.sprite = _healthDepletedSprite;
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
        _slider = GetComponentInChildren<SpriteSlider>();
        _label = GetComponentInChildren<TMP_Text>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }
}