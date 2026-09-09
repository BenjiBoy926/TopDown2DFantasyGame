using DG.Tweening;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterDetailPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _healthLabel;
    [SerializeField] private TMP_Text _powerLabel;
    [SerializeField] private TMP_Text _energyLabel;
    [SerializeField] private TMP_Text _rangeLabel;
    private GameObject _allElements;
    private Character _target;

    private void Awake()
    {
        _allElements = transform.GetChild(0).gameObject;
    }

    public void Populate(Character character)
    {
        SetTarget(character);
    }

    public void Clear()
    {
        SetTarget(null);
    }

    public void Preview(CharacterInfo info)
    {
        ShowHealth(info.Health);
        ShowEnergy(info.Energy);

        if (info.Health != _target.CurrentHealth)
        {
            _healthLabel.DOFade(0, .35f).SetLoops(-1, LoopType.Yoyo);
        }
        if (info.Energy != _target.CurrentEnergy)
        {
            _energyLabel.DOFade(0, .35f).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void ClearPreview()
    {
        Refresh();
    }

    private void SetTarget(Character target)
    {
        _target = target;
        Refresh();
    }

    private void Refresh()
    {
        _healthLabel.DOKill();
        _energyLabel.DOKill();
        _allElements.SetActive(_target);

        if (_target)
        {
            _nameLabel.text = _target.Name;
            _iconImage.sprite = _target.Icon;

            ShowHealth(_target.CurrentHealth);

            _powerLabel.text = _target.CurrentPower.ToString();
            _powerLabel.color = _target.GetPowerColor(_target.CurrentPower);

            ShowEnergy(_target.CurrentEnergy);

            _rangeLabel.text = _target.TraversalRange.ToString();
        }
        else
        {
            _nameLabel.text = string.Empty;
        }
    }

    private void ShowHealth(int health)
    {
        _healthLabel.text = health.ToString();
        _healthLabel.color = _target.GetHealthColor(health);
    }

    private void ShowEnergy(int energy)
    {
        _energyLabel.text = energy.ToString();
        _energyLabel.color = _target.GetEnergyColor(energy);
    }
}