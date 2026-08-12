using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterHealthColor))]
public class CharacterStats : MonoBehaviour
{
    public int BaseHealth => _baseHealth;
    public int TraversalRange => _traversalRange;
    public int CurrentHealth => _currentHealth;
    public int CurrentEnergy => _currentEnergy;
    public bool IsDead => _currentHealth <= 0;
    public int Power => _basePower;

    [SerializeField] private int _baseHealth = 10;
    [SerializeField] private int _basePower = 3;
    [SerializeField] private int _baseEnergy = 1;
    [SerializeField] private int _traversalRange = 3;

    private int _currentHealth;
    private int _currentEnergy = 0;
    private CharacterHealthColor _healthColor;
    private CharacterUI _ui;

    private void Awake()
    {
        _currentHealth = _baseHealth;
        _healthColor = GetComponent<CharacterHealthColor>();
        _ui = GetComponentInChildren<CharacterUI>();
    }

    private void Start()
    {
        _ui.ShowCurrentHealth();
        _ui.ShowPower();
    }

    public void TakeDamageFrom(Character other)
    {
        int newHealth = CalculateHealthAfterHitFrom(other);
        SetHealth(newHealth);
        _ui.ShakeHealthUI();
    }

    public void RestoreHealth()
    {
        SetHealth(_baseHealth);
    }

    public Color GetCurrentHealthColor()
    {
        return GetHealthColor(_currentHealth);
    }

    public Color GetHealthColor(int health)
    {
        return _healthColor.GetColor(health, _baseHealth);
    }

    public void PreviewHealth(int health)
    {
        _ui.PreviewHealth(health);
    }

    public void ClearHealthPreview()
    {
        _ui.ClearHealthPreview();
    }

    public int CalculateHealthAfterHitFrom(Character other)
    {
        int newHealth = _currentHealth - CalculateDamageTakenFrom(other);
        return Mathf.Max(0, newHealth);
    }

    private int CalculateDamageTakenFrom(Character other)
    {
        return other.Power;
    }

    public void SetHealth(int health)
    {
        _currentHealth = Mathf.Max(health, 0);
        _ui.ShowCurrentHealth();
    }

    public void SetEnergy(int energy)
    {
        _currentEnergy = energy;
    }

    public void ChangeEnergy(int delta)
    {
        _currentEnergy += delta;
    }

    public void ZeroEnergy()
    {
        _currentEnergy = Mathf.Min(_currentEnergy, 0);
    }

    public void RefillEnergy()
    {
        _currentEnergy = Mathf.Min(_currentEnergy + _baseEnergy, _baseEnergy);
    }
}