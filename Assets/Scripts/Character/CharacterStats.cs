using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Note: this might be affected by multiple factors like buffs, debuffs, equipment, etc. For now, we just return the base values.
    public int Power => _basePower;

    [SerializeField] private int _baseHealth = 10;
    [SerializeField] private int _basePower = 3;
    private int _currentHealth;

    public void TakeDamageFrom(Character other)
    {
        int newHealth = CalculateHealthAfterHitFrom(other);
        SetHealth(newHealth);
        Debug.Log($"{name} takes damage from {other.name}. Current health: {_currentHealth}");
    }

    private void Awake()
    {
        _currentHealth = _baseHealth;
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
        if (_currentHealth <= 0)
        {
            // ya dead!
        }
    }
}