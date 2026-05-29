using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    private CharacterHealthUI _healthUI;
    private CharacterPowerUI _powerUI;

    private void Awake()
    {
        _healthUI = GetComponentInChildren<CharacterHealthUI>();
        _powerUI = GetComponentInChildren<CharacterPowerUI>();
    }

    public void ShowHealth()
    {
        _healthUI.ShowCurrentHealth();
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