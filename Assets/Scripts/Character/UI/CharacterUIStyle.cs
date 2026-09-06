using UnityEngine;

[CreateAssetMenu]
public class CharacterUIStyle : ScriptableObject
{
    public Color EnergyNormalColor => _energyNormalColor;
    public Color EnergyEmptyColor => _energyEmptyColor;
    public Color EnergyNegativeColor => _energyNegativeColor;

    [SerializeField] private Sprite _heartFullSprite;
    [SerializeField] private Sprite _heartDamageSprite;
    [SerializeField] private Sprite _heartEmptySprite;
    [SerializeField] private Sprite _xSprite;
    [SerializeField] private float _healthHalfThreshold = .5f;
    [SerializeField] private float _healthLowThreashold = .2f;
    [SerializeField] private Color _healthFullColor = Color.green;
    [SerializeField] private Color _healthHalfColor = Color.yellow;
    [SerializeField] private Color _healthLowColor = Color.red;

    [Space]
    [SerializeField] private Color _powerHighColor = Color.green;
    [SerializeField] private Color _powerNormalColor = Color.white;
    [SerializeField] private Color _powerLowColor = Color.red;

    [Space]
    [SerializeField] private Color _energyNormalColor = Color.yellow;
    [SerializeField] private Color _energyEmptyColor = Color.gray;
    [SerializeField] private Color _energyNegativeColor = Color.red;

    public Sprite GetHeartSprite(int currentHealth, int baseHealth)
    {
        if (currentHealth >= baseHealth)
        {
            return _heartFullSprite;
        }
        else if (currentHealth > 0)
        {
            return _heartDamageSprite;
        }
        else
        {
            return _heartEmptySprite;
        }
    }

    public Sprite GetXSprite(int currentHealth)
    {
        return currentHealth <= 0 ? _xSprite : null;
    }

    public Color GetHealthColor(int currentHealth, int baseHealth)
    {
        int halfHealthThreshold = Mathf.CeilToInt(baseHealth * _healthHalfThreshold);
        int lowHealthThreshold = Mathf.CeilToInt(baseHealth * _healthLowThreashold);
        if (currentHealth > halfHealthThreshold)
        {
            return _healthFullColor;
        }
        else if (currentHealth > lowHealthThreshold)
        {
            return _healthHalfColor;
        }
        else
        {
            return _healthLowColor;
        }
    }

    public Color GetPowerColor(int currentPower, int basePower)
    {
        if (currentPower > basePower)
        {
            return _powerHighColor;
        }
        else if (currentPower == basePower)
        {
            return _powerNormalColor;
        }
        else
        {
            return _powerLowColor;
        }
    }

    public Color GetEnergyColor(int currentEnergy)
    {
        if (currentEnergy > 0)
        {
            return _energyNormalColor;
        }
        else if (currentEnergy == 0)
        {
            return _energyEmptyColor;
        }
        else
        {
            return _energyNegativeColor;
        }
    }
}