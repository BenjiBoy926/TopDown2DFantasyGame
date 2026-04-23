using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Note: this might be affected by multiple factors like buffs, debuffs, equipment, etc. For now, we just return the base values.
    public int Power => _basePower;
    public bool IsDead => _currentHealth <= 0;

    [SerializeField] private int _baseHealth = 10;
    [SerializeField] private int _basePower = 3;
    private int _currentHealth;
    private CharacterStatsUI _ui;

    public void TakeDamageFrom(Character other)
    {
        int newHealth = CalculateHealthAfterHitFrom(other);
        SetHealth(newHealth);
        _ui.ShakeHealthUI();
    }

    public void FadeInUI()
    {
        _ui.FadeIn();
    }

    public void FadeOutUI()
    {
        _ui.FadeOut();
    }

    private void Awake()
    {
        _currentHealth = _baseHealth;
        _ui = GetComponentInChildren<CharacterStatsUI>();
        _ui.ShowHealth(_currentHealth, _baseHealth);
        _ui.ShowPower(Power);
    }

    private int CalculateHealthAfterHitFrom(Character other)
    {
        return _currentHealth - CalculateDamageTakenFrom(other);
    }

    private int CalculateDamageTakenFrom(Character other)
    {
        return other.Power;
    }

    private void SetHealth(int health)
    {
        _currentHealth = Mathf.Max(health, 0);
        _ui.ShowHealth(_currentHealth, _baseHealth);
    }
}