using System.Collections.Generic;
using UnityEngine;

public class BattleTurn : MonoBehaviour
{
    public Faction CurrentFaction => _currentFactionIndex >= 0 ? _factions[_currentFactionIndex] : null;
    public bool IsAnimationPlaying => _animation && _animation.IsPlaying;

    [SerializeField] private BattleTurnChangeAnimation _animation;
    [SerializeField] private Faction _startingFaction;
    private readonly HashSet<Character> _characters = new();
    private readonly List<Faction> _factions = new(2);
    private int _currentFactionIndex = -1;
    private readonly List<Character> _characterListScratch = new();

    public void Register(Character obj)
    {
        _characters.Add(obj);

        Faction faction = obj.Faction;
        if (!_factions.Contains(faction))
        {
            _factions.Add(faction);
        }
    }

    public void Unregister(Character obj)
    {
        _characters.Remove(obj);
        GetCharactersInFaction(obj.Faction, _characterListScratch);
        if (_characterListScratch.Count == 0)
        {
            _factions.Remove(obj.Faction);
        }
    }

    public void StartFirstTurn()
    {
        StartTurn(_startingFaction);
    }

    public void StartNextTurn()
    {
        StartTurn(_currentFactionIndex + 1);
        Debug.Log($"Starting turn for {CurrentFaction}");
    }

    private void StartTurn(Faction faction)
    {
        int index = _factions.IndexOf(faction);
        StartTurn(index);
    }

    private void StartTurn(int factionIndex)
    {
        _currentFactionIndex = factionIndex % _factions.Count;
        foreach (Character character in _characters)
        {
            character.RestoreMove();
        }
        _animation.Play(CurrentFaction);
    }

    private void OnEnable()
    {
        Character.MoveFinished += OnCharacterMoveFinished;
    }

    private void OnDisable()
    {
        Character.MoveFinished -= OnCharacterMoveFinished;
    }

    private void OnCharacterMoveFinished(Character obj)
    {
        if (CountCharactersThatCanStillMove(CurrentFaction) == 0)
        {
            StartNextTurn();
        }
    }

    private int CountCharactersThatCanStillMove(Faction faction)
    {
        GetCharactersInFaction(faction, _characterListScratch);

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
