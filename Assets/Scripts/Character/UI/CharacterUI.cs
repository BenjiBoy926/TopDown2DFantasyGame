using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    private static readonly HashSet<CharacterUI> _instances = new();

    [SerializeField] private float _hideAlpha = .1f;
    private CharacterHealthUI _healthUI;
    private CharacterPowerUI _powerUI;
    private SpriteRenderer[] _renderers;
    private TMP_Text[] _labels;

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
        foreach (var renderer in _renderers)
        {
            renderer.DOKill();
            renderer.DOFade(_hideAlpha, 0.1f);
        }
    }

    public void Show()
    {
        foreach (var renderer in _renderers)
        {
            renderer.DOKill();
            renderer.DOFade(1, 0.1f);
        }
    }

    private void Awake()
    {
        _healthUI = GetComponentInChildren<CharacterHealthUI>();
        _powerUI = GetComponentInChildren<CharacterPowerUI>();
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _labels = GetComponentsInChildren<TMP_Text>();
        _instances.Add(this);
    }

    private void OnDestroy()
    {
        _instances.Remove(this);
    }
}