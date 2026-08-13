using UnityEngine;

public class CharacterEnergyUI : MonoBehaviour
{
    private Character _character;
    private CharacterEnergyNotch[] _energyNotches;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _energyNotches = GetComponentsInChildren<CharacterEnergyNotch>();
    }

    public void AnimateCurrentEnergy()
    {
        AnimateEnergy(_character.CurrentEnergy);
    }

    public void AnimateEnergy(int energy)
    {
        Debug.Log("Animating energy: " + energy);
    }

    public void ShowCurrentEnergy()
    {
        SetEnergy(_character.CurrentEnergy);
    }

    public void SetEnergy(int energy)
    {
        Debug.Log("Setting energy: " + energy);
    }
}