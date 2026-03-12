using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battlefield))]
public class Battle : MonoBehaviour
{
    private Faction CurrentFaction => _factions[_currentFactionIndex];

    private Battlefield _field;
    private readonly HashSet<Character> _characters = new();
    private readonly List<Faction> _factions = new(2);
    private int _currentFactionIndex = 0;
    private readonly List<Character> _characterListScratch = new();

    public void Register(Character obj)
    {
        _characters.Add(obj);
        _field.Register(obj);

        Faction faction = obj.Faction;
        if (!_factions.Contains(faction))
        {
            _factions.Add(faction);
        }
    }

    public void Unregister(Character obj)
    {
        _characters.Remove(obj);
        _field.Unregister(obj);
        GetCharactersInFaction(obj.Faction, _characterListScratch);
        if (_characterListScratch.Count == 0)
        {
            _factions.Remove(obj.Faction);
        }
    }

    public void StartNextTurn()
    {
        _currentFactionIndex = (_currentFactionIndex + 1) % _factions.Count;
        foreach (Character character in _characters)
        {
            character.RestoreMove();
        }
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        return _field.SnapToGrid(position);
    }

    public Vector3Int WorldToCellRounded(Vector3 position)
    {
        return _field.WorldToCellRounded(position);
    }

    public Character GetOccupant(Vector3Int cell)
    {
        return _field.GetOccupant(cell);
    }

    public void RefreshOccupantCell(Character character)
    {
        _field.RefreshOccupantCell(character);
    }

    private void Awake()
    {
        _field = GetComponent<Battlefield>();
    }

    private void OnEnable()
    {
        Character.UsedMove += OnCharacterUsedMove;
    }

    private void OnDisable()
    {
        Character.UsedMove -= OnCharacterUsedMove;
    }

    private void OnCharacterUsedMove(Character obj)
    {
        if (CountCharactersThatCanStillMove(CurrentFaction) == 0)
        {
            StartNextTurn();
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
        foreach (Character character in _characters)
        {
            if (character.Faction == faction)
            {
                characters.Add(character);
            }
        }
    }
}
