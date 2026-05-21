using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleTurn : MonoBehaviour
{
    public Faction CurrentFaction => _currentFactionIndex >= 0 ? _factions[_currentFactionIndex] : null;
    public Faction StartingFaction => _startingFaction;
    public bool IsAnimationPlaying => _animation && _animation.IsPlaying;

    [SerializeField] private Faction _startingFaction;
    private readonly List<Faction> _factions = new(2);
    private int _currentFactionIndex = -1;
    private readonly List<Character> _characterListScratch = new();
    private Battle _battle;
    private BattleTurnChangeAnimation _animation;

    private void Awake()
    {
        _battle = GetComponent<Battle>();
        _animation = GetComponentInChildren<BattleTurnChangeAnimation>();
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
        if (CountMoveableCharacters(CurrentFaction) == 0)
        {
            StartNextTurn();
        }
    }

    public void AddFaction(Character obj)
    {
        Faction faction = obj.Faction;
        if (!_factions.Contains(faction))
        {
            _factions.Add(faction);
        }
    }

    public void StartFirstTurn()
    {
        SetCurrentTurn(_startingFaction);
        PlayTurnChangeAnimation();
    }

    public void StartNextTurn()
    {
        RestoreAllCharacterMoves();

        GetCharactersInFaction(CurrentFaction, _characterListScratch);
        List<Character> charactersToRecord = new(_characterListScratch);

        int nextFactionIndex = GetNextFactionWithMoveableCharacters();
        SetCurrentTurn(nextFactionIndex);
        PlayTurnChangeAnimation();

        _battle.RecordTurnChange(charactersToRecord);
    }

    public void PlayTurnChangeAnimation()
    {
        _animation.Play(CurrentFaction);
    }

    public void SetCurrentTurn(Faction faction)
    {
        int index = _factions.IndexOf(faction);
        SetCurrentTurn(index);
    }

    private int GetNextFactionWithMoveableCharacters()
    {
        int nextFactionIndex = GetNextFactionIndex(_currentFactionIndex);
        Faction nextFaction = _factions[nextFactionIndex];

        int iterations = 0;
        int maxIterations = 5;

        while (CountMoveableCharacters(nextFaction) <= 0 && iterations < maxIterations)
        {
            nextFactionIndex = GetNextFactionIndex(nextFactionIndex);
            nextFaction = _factions[nextFactionIndex];
            iterations++;
        }

        if (iterations >= maxIterations)
        {
            throw new System.Exception($"There are no moveable characters in any factions in the battle, so we cannot start the next turn");
        }
        return nextFactionIndex;
    }

    private int GetNextFactionIndex(int currentFaction)
    {
        return (currentFaction + 1) % _factions.Count;
    }

    private void SetCurrentTurn(int factionIndex)
    {
        _currentFactionIndex = factionIndex;
    }

    private void RestoreAllCharacterMoves()
    {
        foreach (Character character in _battle.AllCharacters)
        {
            character.RestoreMove();
        }
    }

    private int CountMoveableCharacters(Faction faction)
    {
        GetCharactersInFaction(faction, _characterListScratch);

        int canStillMove = 0;
        for (int i = 0; i < _characterListScratch.Count; i++)
        {
            Character character = _characterListScratch[i];
            if (character.IsAbleToMove)
            {
                canStillMove++;
            }
        }
        return canStillMove;
    }

    private void GetCharactersInFaction(Faction faction, List<Character> characters)
    {
        characters.Clear();
        foreach (Character character in _battle.AllCharacters)
        {
            if (character.Faction == faction)
            {
                characters.Add(character);
            }
        }
    }
}
