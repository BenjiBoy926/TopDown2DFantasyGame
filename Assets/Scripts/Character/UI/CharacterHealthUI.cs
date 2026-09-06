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

    private Character _character;
    private CharacterHealthbar _healthBar;
    private CharacterHealthbarPreview _healthBarPreview;
    private CharacterHeartIcon _heartIcon;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _healthBar = GetComponentInChildren<CharacterHealthbar>();
        _healthBarPreview = GetComponentInChildren<CharacterHealthbarPreview>(true);
        _heartIcon = GetComponentInChildren<CharacterHeartIcon>();
    }

    public void Preview(CharacterInfo info)
    {
        int higher = Mathf.Max(info.Health, _character.CurrentHealth);
        int lower = Mathf.Min(info.Health, _character.CurrentHealth);
        
        ShowHealth(info.Health);
        _healthBar.ShowHealth(lower);

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
        ShowHealth(_character.CurrentHealth);
    }

    public void ShowHealth(int health)
    {
        _healthBar.ShowHealth(health);
        ShowHealthOnHeartIcon(health);
    }

    public void ShowHealthOnHeartIcon(int health)
    {
        _heartIcon.ShowHealth(_character.UIStyle, health, _character.BaseHealth);
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