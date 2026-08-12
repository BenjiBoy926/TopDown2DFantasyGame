using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    private CharacterHealthUI _healthUI;
    private CharacterPowerUI _powerUI;
    private CharacterEnergyUI _energyUI;

    private void Awake()
    {
        _healthUI = GetComponentInChildren<CharacterHealthUI>();
        _powerUI = GetComponentInChildren<CharacterPowerUI>();
        _energyUI = GetComponentInChildren<CharacterEnergyUI>();
    }

    public void PreviewHealth(int health)
    {
        _healthUI.Preview(health);
    }

    public void ClearHealthPreview()
    {
        _healthUI.ClearPreview();
    }

    public void ShowCurrentHealth()
    {
        _healthUI.ShowCurrentHealth();
    }

    public void AnimateCurrentEnergy()
    {
        _energyUI.AnimateCurrentEnergy();
    }

    public void ShowCurrentEnergy()
    {
        _energyUI.ShowCurrentEnergy();
    }

    public void ShowPower()
    {
        if (_powerUI)
        {
            _powerUI.ShowPower();
        }
    }

    public void ShakeHealthUI()
    {
        _healthUI.Shake();
    }
}