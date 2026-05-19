using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Note: this might be affected by multiple factors like buffs, debuffs, equipment, etc. For now, we just return the base values.
    public int Power => _basePower;
    public bool IsDead => _currentHealth <= 0;
    public int TraversalRange => _traversalRange;
    public int CurrentHealth => _currentHealth;

    [SerializeField] private int _baseHealth = 10;
    [SerializeField] private int _basePower = 3;
    [SerializeField] private int _traversalRange = 3;
    private int _currentHealth;
    private CharacterUI _ui;

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

    public void FadeInUI()
    {
        _ui.Show();
    }

    public void FadeOutUI()
    {
        _ui.Hide();
    }

    private void Awake()
    {
        _currentHealth = _baseHealth;
        _ui = GetComponentInChildren<CharacterUI>();
    }

    private void Start()
    {
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

    public void SetHealth(int health)
    {
        _currentHealth = Mathf.Max(health, 0);
        _ui.ShowHealth(_currentHealth, _baseHealth);
    }
}