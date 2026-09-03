using TMPro;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharacterHealthColor))]
public class CharacterStats : MonoBehaviour
{
    public int BaseHealth => _baseHealth;
    public int BaseEnergy => _baseEnergy;
    public int CurrentHealth => _currentHealth;
    public int CurrentPower => _basePower;
    public int CurrentEnergy => _currentEnergy;
    public int TraversalRange => _traversalRange;
    public bool IsDead => _currentHealth <= 0;

    [SerializeField] private int _baseHealth = 10;
    [SerializeField] private int _basePower = 3;
    [SerializeField] private int _baseEnergy = 1;
    [SerializeField] private int _traversalRange = 3;

    private int _currentHealth;
    private int _currentEnergy = 0;
    private Character _character;
    private CharacterHealthColor _healthColor;

    private void Awake()
    {
        _currentHealth = _baseHealth;
        _character = GetComponent<Character>();
        _healthColor = GetComponent<CharacterHealthColor>();
    }

    private void Start()
    {
        _character.ShowCurrentHealth();
        _character.ShowPower();
        _character.ShowCurrentEnergy();
    }

    public void TakeDamageFrom(Character other)
    {
        int newHealth = _currentHealth - other.CurrentPower;
        SetHealth(newHealth);
        _character.ShakeHealthUI();
    }

    public void RestoreHealth()
    {
        SetHealth(_baseHealth);
    }

    public Color GetHealthColor(int health)
    {
        return _healthColor.GetColor(health, _baseHealth);
    }

    public void SetHealth(int health)
    {
        _currentHealth = Mathf.Max(health, 0);
        _character.ShowCurrentHealth();
    }

    public void ChangeEnergy(int delta)
    {
        SetEnergy(_currentEnergy + delta);
    }

    public void ZeroEnergy()
    {
        SetEnergy(Mathf.Min(_currentEnergy, 0));
    }

    public void RefillEnergy()
    {
        SetEnergy(Mathf.Min(_currentEnergy + _baseEnergy, _baseEnergy));
    }

    public void SetEnergy(int energy)
    {
        if (energy == _currentEnergy)
            return;

        _currentEnergy = energy;
        _character.AnimateCurrentEnergy();
    }
}