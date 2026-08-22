using UnityEngine;
using UnityEngine.InputSystem;

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
        for (int i = 0; i < _energyNotches.Length; i++)
        {
            CharacterEnergyNotch notch = _energyNotches[i];
            CharacterEnergyNotch.State targetState = GetTargetState(energy, i);
            notch.AnimateState(targetState);
        }
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
        if (notchIndex >= _character.BaseEnergy && notchIndex >= Mathf.Abs(energy))
        {
            return CharacterEnergyNotch.State.Invisible;
        }
        else if (energy < 0)
        {
            return GetNegativeState(energy, notchIndex);
        }
        else
        {
            return GetPositiveState(energy, notchIndex);
        }
    }

    private CharacterEnergyNotch.State GetPositiveState(int energy, int notchIndex)
    {
        if (notchIndex < energy)
        {
            return CharacterEnergyNotch.State.Filled;
        }
        else
        {
            return CharacterEnergyNotch.State.Empty;
        }
    }

    private CharacterEnergyNotch.State GetNegativeState(int energy, int notchIndex)
    {
        energy = Mathf.Abs(energy);
        if (notchIndex < energy)
        {
            return CharacterEnergyNotch.State.Negative;
        }
        else
        {
            return CharacterEnergyNotch.State.Empty;
        }
    }
}