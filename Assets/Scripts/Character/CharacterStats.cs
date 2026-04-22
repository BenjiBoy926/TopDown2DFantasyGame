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
        // Shake the health ui
    }

    private void Awake()
    {
        _currentHealth = _baseHealth;
        _ui = GetComponentInChildren<CharacterStatsUI>();
        _ui.ShowHealth(_currentHealth, _baseHealth);
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
        _currentHealth = health;
        _ui.ShowHealth(health, _baseHealth);
    }
}