using TMPro;
using UnityEngine;

public class CharacterPowerUI : MonoBehaviour
{
    private CharacterStats _stats;
    private TMP_Text _label;

    public void ShowPower()
    {
        _label.text = _stats.Power.ToString();
    }

    private void Awake()
    {
        _stats = GetComponentInParent<CharacterStats>();
        _label = GetComponentInChildren<TMP_Text>();
    }
}