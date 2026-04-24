using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    private static readonly HashSet<CharacterUI> _instances = new();

    private CharacterHealthUI _healthUI;
    private CharacterPowerUI _powerUI;

    public static void HideAll()
    {
        foreach (var instance in _instances)
        {
            instance.Hide();
        }
    }

    public static void ShowAll()
    {
        foreach (var instance in _instances)
        {
            instance.Show();
        }
    }

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

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        _healthUI = GetComponentInChildren<CharacterHealthUI>();
        _powerUI = GetComponentInChildren<CharacterPowerUI>();
        _instances.Add(this);
    }

    private void OnDestroy()
    {
        _instances.Remove(this);
    }
}