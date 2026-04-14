using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int Power => _power;

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _power;
    private int _currentHealth;

    public void TakeDamageFrom(Character other)
    {
        int damage = CalculateDamageTakenFrom(other);
        SetHealth(_currentHealth - damage);
        Debug.Log($"{name} takes {damage} damage from {other.name}. Current health: {_currentHealth}");
    }

    private void Awake()
    {
        _currentHealth = _maxHealth;
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