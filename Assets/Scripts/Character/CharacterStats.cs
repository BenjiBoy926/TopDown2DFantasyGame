using UnityEngine;

[RequireComponent(typeof(CharacterHealthColor))]
public class CharacterStats : MonoBehaviour
{
    public int BaseHealth => _baseHealth;
    public int TraversalRange => _traversalRange;
    public int CurrentHealth => _currentHealth;
    public bool IsDead => _currentHealth <= 0;
    public int Power => _basePower;

    [SerializeField] private int _baseHealth = 10;
    [SerializeField] private int _basePower = 3;
    [SerializeField] private int _traversalRange = 3;

    private int _currentHealth;
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
        _ui.ShowHealth();
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

    public Color GetHealthColor()
    {
        return _healthColor.GetColor();
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
        _ui.ShowHealth();
    }
}