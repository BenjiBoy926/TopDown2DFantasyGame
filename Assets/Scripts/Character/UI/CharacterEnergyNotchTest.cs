using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(CharacterEnergyNotch))]
public class CharacterEnergyNotchTest : MonoBehaviour
{
    [SerializeField] private CharacterEnergyNotch.State _state;
    private CharacterEnergyNotch _notch;

    private void Awake()
    {
        _notch = GetComponent<CharacterEnergyNotch>();
    }

    [Button]
    private void Animate()
    {
        _notch.AnimateState(_state);
    }

    [Button]
    private void Set()
    {
        _notch.SetState(_state);
    }
}