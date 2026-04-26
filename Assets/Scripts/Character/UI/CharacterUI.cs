using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    private struct LabelAlphaTween
    {
        public TMP_Text _label;

        public LabelAlphaTween(TMP_Text label, float alpha, float duration)
        {
            _label = label;
            label.DOKill();
            DOTween.To(Get, Set, alpha, duration).SetTarget(label);
        }

        public readonly float Get()
        {
            return _label.color.a;
        }

        public readonly void Set(float value)
        {
            Color color = _label.color;
            color.a = value;
            _label.color = color;
        }
    }

    private static readonly HashSet<CharacterUI> _instances = new();
    private const float FadeDuration = .1f;

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
        for (int i = 0; i < _renderers.Length; i++)
        {
            SpriteRenderer renderer = _renderers[i];
            renderer.DOKill();
            renderer.DOFade(_hideAlpha, FadeDuration);
        }
        for (int i = 0; i < _labels.Length; i++)
        {
            TMP_Text label = _labels[i];
            new LabelAlphaTween(label, _hideAlpha, FadeDuration);
        }
    }

    public void Show()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            SpriteRenderer renderer = _renderers[i];
            renderer.DOKill();
            renderer.DOFade(1, .1f);
        }
        for (int i = 0; i < _labels.Length; i++)
        {
            TMP_Text label = _labels[i];
            new LabelAlphaTween(label, 1, FadeDuration);
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