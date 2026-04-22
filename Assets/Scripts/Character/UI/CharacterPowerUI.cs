using TMPro;
using UnityEngine;

public class CharacterPowerUI : MonoBehaviour
{
    private TMP_Text _label;

    public void ShowPower(int power)
    {
        _label.text = power.ToString();
    }

    private void Awake()
    {
        _label = GetComponentInChildren<TMP_Text>();
    }
}