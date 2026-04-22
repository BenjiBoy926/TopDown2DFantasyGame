using TMPro;
using UnityEngine;

public class CharacterStatsUI : MonoBehaviour
{
    private CharacterHealthUI _healthUI;

    public void ShowHealth(int currentHealth, int baseHealth)
    {
        _healthUI.ShowHealth(currentHealth, baseHealth);
    }

    public void ShakeHealthUI()
    {
        _healthUI.Shake();
    }

    private void Awake()
    {
        _healthUI = GetComponentInChildren<CharacterHealthUI>();
    }
}