using TMPro;
using UnityEngine;

public class CharacterDetailPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthLabel;
    [SerializeField] private TMP_Text _powerLabel;
    [SerializeField] private TMP_Text _energyLabel;
    [SerializeField] private TMP_Text _rangeLabel;

    public void Populate(Character character)
    {
        _healthLabel.text = character.CurrentHealth.ToString();
        _powerLabel.text = character.CurrentPower.ToString();
        _energyLabel.text = character.CurrentEnergy.ToString();
        _rangeLabel.text = character.TraversalRange.ToString();
    }
}