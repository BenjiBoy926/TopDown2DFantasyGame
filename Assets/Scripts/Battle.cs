using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battlefield))]
public class Battle : MonoBehaviour
{
    private Battlefield _field;
    private readonly List<Faction> _factions = new(2);
    private int _currentFaction = 0;
    private readonly List<Character> _characterListScratch = new();

    private void Awake()
    {
        _field = GetComponent<Battlefield>();
    }

    private void OnEnable()
    {
        _field.CharacterAdded += OnCharacterAdded;
        _field.CharacterRemoved += OnCharacterRemoved;
        Character.UsedMove += OnCharacterUsedMove;
        LoadFactions();
    }

    private void OnDisable()
    {
        _field.CharacterAdded -= OnCharacterAdded;
        _field.CharacterRemoved -= OnCharacterRemoved;
        Character.UsedMove -= OnCharacterUsedMove;
    }

    private void LoadFactions()
    {
        _factions.Clear();
        foreach (Character character in _field.Characters)
        {
            AddCharacterFaction(character);
        }
    }

    private void OnCharacterAdded(Character obj)
    {
        AddCharacterFaction(obj);
    }

    private void OnCharacterRemoved(Character obj)
    {
        GetCharactersInFaction(obj.Faction, _characterListScratch);
        if (_characterListScratch.Count == 0)
        {
            _factions.Remove(obj.Faction);
        }
    }

    private void OnCharacterUsedMove(Character obj)
    {
        // If all characters in current faction used their move then start the next faction's turn
        Debug.Log($"Character '{obj}' used move. Faction count: {_factions.Count}", obj);
    }

    private void AddCharacterFaction(Character obj)
    {
        Faction faction = obj.Faction;
        if (!_factions.Contains(faction))
        {
            _factions.Add(faction);
        }
    }

    private void GetCharactersInFaction(Faction faction, List<Character> characters)
    {
        characters.Clear();
        foreach (Character character in _field.Characters)
        {
            if (character.Faction == faction)
            {
                characters.Add(character);
            }
        }
    }
}
