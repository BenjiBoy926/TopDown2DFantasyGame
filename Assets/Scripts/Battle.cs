using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battlefield))]
public class Battle : MonoBehaviour
{
    private Faction CurrentFaction => _factions[_currentFactionIndex];

    private Battlefield _field;
    private readonly List<Faction> _factions = new(2);
    private int _currentFactionIndex = 0;
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
        if (CountCharactersThatCanStillMove(CurrentFaction) == 0)
        {
            StartNextTurn();
        }
    }

    private void StartNextTurn()
    {
        _currentFactionIndex = (_currentFactionIndex + 1) % _factions.Count;
        foreach (Character character in _field.Characters)
        {
            character.RestoreMove();
        }
    }

    private void AddCharacterFaction(Character obj)
    {
        Faction faction = obj.Faction;
        if (!_factions.Contains(faction))
        {
            _factions.Add(faction);
        }
    }

    private int CountCharactersThatCanStillMove(Faction faction)
    {
        GetCharactersInFaction(CurrentFaction, _characterListScratch);

        int canStillMove = 0;
        for (int i = 0; i < _characterListScratch.Count; i++)
        {
            Character character = _characterListScratch[i];
            if (!character.HasMovedThisTurn)
            {
                canStillMove++;
            }
        }
        return canStillMove;
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
