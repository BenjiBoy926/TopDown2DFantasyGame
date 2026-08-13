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
        // No animations yet
        SetEnergy(energy);
    }

    public void ShowCurrentEnergy()
    {
        SetEnergy(_character.CurrentEnergy);
    }

    public void SetEnergy(int energy)
    {
        for (int i = 0; i < _energyNotches.Length; i++)
        {
            CharacterEnergyNotch notch = _energyNotches[i];
            CharacterEnergyNotch.State targetState = GetTargetState(energy, i);
            notch.SetState(targetState);
        }
    }

    private CharacterEnergyNotch.State GetTargetState(int energy, int notchIndex)
    {
        if (notchIndex >= _character.BaseEnergy)
        {
            return CharacterEnergyNotch.State.Invisible;
        }
        else
        {
            return CharacterEnergyNotch.State.Filled;
        }
    }
}