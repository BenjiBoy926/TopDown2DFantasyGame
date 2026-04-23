using UnityEngine;

public class CharacterStatsUI : MonoBehaviour
{
    private CharacterHealthUI _healthUI;
    private CharacterPowerUI _powerUI;

    public void ShowHealth(int currentHealth, int baseHealth)
    {
        _healthUI.ShowHealth(currentHealth, baseHealth);
    }

    public void ShowPower(int power)
    {
        _powerUI.ShowPower(power);
    }

    public void ShakeHealthUI()
    {
        _healthUI.Shake();
    }

    public void FadeOut()
    {
        gameObject.SetActive(false);
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        _healthUI = GetComponentInChildren<CharacterHealthUI>();
        _powerUI = GetComponentInChildren<CharacterPowerUI>();
    }
}