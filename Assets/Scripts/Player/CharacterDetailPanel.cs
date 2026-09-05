using TMPro;
using UnityEngine;

public class CharacterDetailPanel : MonoBehaviour
{
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
        _healthLabel.text = character.CurrentHealth.ToString();
        _powerLabel.text = character.CurrentPower.ToString();
        _energyLabel.text = character.CurrentEnergy.ToString();
        _rangeLabel.text = character.TraversalRange.ToString();
    }

    public void Clear()
    {
        _allElements.SetActive(false);
    }
}