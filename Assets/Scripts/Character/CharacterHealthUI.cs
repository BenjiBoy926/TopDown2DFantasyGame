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

    private TMP_Text _healthText;
    private SpriteRenderer _renderer;

    public void ShowHealth(int currentHealth, int baseHealth)
    {
        _healthText.text = currentHealth.ToString();
        if (currentHealth == baseHealth)
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
        _healthText = GetComponentInChildren<TMP_Text>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }
}