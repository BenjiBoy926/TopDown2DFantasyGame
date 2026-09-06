using TMPro;
using UnityEngine;
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

    private void Awake()
    {
        _allElements = transform.GetChild(0).gameObject;
    }

    public void Populate(Character character)
    {
        _allElements.SetActive(true);

        _nameLabel.text = character.Name;
        _iconImage.sprite = character.Icon;

        _healthLabel.text = character.CurrentHealth.ToString();
        _healthLabel.color = character.GetHealthColor(character.CurrentHealth);

        _powerLabel.text = character.CurrentPower.ToString();
        _powerLabel.color = character.GetPowerColor(character.CurrentPower);

        _energyLabel.text = character.CurrentEnergy.ToString();
        _energyLabel.color = character.GetEnergyColor(character.CurrentEnergy);

        _rangeLabel.text = character.TraversalRange.ToString();
    }

    public void Clear()
    {
        _nameLabel.text = string.Empty;
        _allElements.SetActive(false);
    }
}