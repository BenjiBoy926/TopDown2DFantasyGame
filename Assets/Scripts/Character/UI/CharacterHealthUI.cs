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
    private bool _isPreviewing = false;
    private Vector3 _originalLocalPosition;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _healthBar = GetComponentInChildren<CharacterHealthbar>();
        _healthBarPreview = GetComponentInChildren<CharacterHealthbarPreview>(true);
        _heartIcon = GetComponentInChildren<CharacterHeartIcon>();
        _originalLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (_isPreviewing)
        {
            // May need an offset, also may need to change render layer to be above character
            Vector2 cellPosition = _character.CurrentCellCenter;
            transform.position = cellPosition;
        }
    }

    public void Preview(int health)
    {
        int higher = Mathf.Max(health, _character.CurrentHealth);
        int lower = Mathf.Min(health, _character.CurrentHealth);
        _healthBar.ShowHealth(lower);
        ShowHealthOnHeartIcon(health);
        _healthBarPreview.Show();
        _healthBarPreview.SetFill(higher);
        _isPreviewing = true;
    }

    public void ClearPreview()
    {
        _healthBarPreview.Hide();
        ShowCurrentHealth();
        transform.localPosition = _originalLocalPosition;
        _isPreviewing = false;
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
        int baseHealth = _character.BaseHealth;
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