using System;
using UnityEngine;

[Serializable]
public struct CharacterRecord
{
    public readonly Character Character => _character;
    public readonly CharacterState State => _state;

    [SerializeField] private Character _character;
    [SerializeField] private CharacterState _state;

    public CharacterRecord(Character character, CharacterState state)
    {
        _character = character;
        _state = state;
    }

    public static CharacterRecord Read(Character character)
    {
        return new(character, character.ReadState());
    }

    public readonly void Write()
    {
        Character.ApplyState(State);
    }
}