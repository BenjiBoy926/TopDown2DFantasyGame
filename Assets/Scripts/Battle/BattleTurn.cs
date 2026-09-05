using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleTurn : MonoBehaviour
{
    public static Action<Faction> NextTurnStarted = delegate { };

    public Faction CurrentFaction => _currentFactionIndex >= 0 ? _factions[_currentFactionIndex] : null;
    public bool IsAnimationPlaying => _animation && _animation.IsPlaying;

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

    public void StartNextTurn()
    {
        _battle.SetPlayerPosition(_battle.PlayerCommanderPosition);

        int nextFactionIndex = GetNextFactionWithLivingCharacters();
        SetCurrentTurn(nextFactionIndex);
        PlayTurnChangeAnimation();

        if (CurrentFaction == _battle.PlayerFaction)
        {
            _battle.CameraGlide(_battle.PlayerCommanderTransform);
        }
        _battle.RecordTurnChange(_battle.AllCharacters, CurrentFaction);
        NextTurnStarted.Invoke(CurrentFaction);
    }

    public void StartTurn(Faction faction)
    {
        SetCurrentTurn(faction);
        PlayTurnChangeAnimation();
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

    private int GetNextFactionWithLivingCharacters()
    {
        int nextFactionIndex = GetNextFactionIndex(_currentFactionIndex);
        Faction nextFaction = _factions[nextFactionIndex];

        int iterations = 0;
        int maxIterations = 5;

        while (CountLivingCharacters(nextFaction) <= 0 && iterations < maxIterations)
        {
            nextFactionIndex = GetNextFactionIndex(nextFactionIndex);
            nextFaction = _factions[nextFactionIndex];
            iterations++;
        }

        if (iterations >= maxIterations)
        {
            throw new Exception($"There are no moveable characters in any factions in the battle, so we cannot start the next turn");
        }
        return nextFactionIndex;
    }

    private int GetNextFactionIndex(int currentFaction)
    {
        return (currentFaction + 1) % _factions.Count;
    }

    private void SetCurrentTurn(int factionIndex)
    {
        if (_currentFactionIndex == factionIndex) 
            return;
        
        ZeroEnergyOfCharactersInFaction(_currentFactionIndex);
        _currentFactionIndex = factionIndex;
        RefillEnergyOfCharactersInFaction(_currentFactionIndex);
        foreach (var character in _battle.AllCharacters)
        {
            character.FadeAppearanceToTargetState();
        }
    }

    private void ZeroEnergyOfCharactersInFaction(int factionIndex)
    {
        if (factionIndex < 0 || factionIndex >= _factions.Count) 
            return;

        Faction faction = _factions[factionIndex];
        GetCharactersInFaction(faction, _characterListScratch);
        for (int i = 0; i < _characterListScratch.Count; i++)
        {
            Character character = _characterListScratch[i];
            character.ZeroEnergy();
        }
    }

    private void RefillEnergyOfCharactersInFaction(int factionIndex)
    {
        if (factionIndex < 0 || factionIndex >= _factions.Count)
            return;

        Faction faction = _factions[factionIndex];
        GetCharactersInFaction(faction, _characterListScratch);
        for (int i = 0; i < _characterListScratch.Count; i++)
        {
            Character character = _characterListScratch[i];
            character.RefillEnergy();
        }
    }

    public int CountMoveableCharacters(Faction faction)
    {
        return CountCharacters(faction, character => character.IsAbleToMove);
    }

    public int CountLivingCharacters(Faction faction)
    {
        return CountCharacters(faction, character => !character.IsDead);
    }

    private int CountCharacters(Faction faction, Predicate<Character> predicate)
    {
        GetCharactersInFaction(faction, _characterListScratch);

        int count = 0;
        for (int i = 0; i < _characterListScratch.Count; i++)
        {
            Character character = _characterListScratch[i];
            if (predicate(character))
            {
                count++;
            }
        }
        return count;
    }

    public void GetCharactersInFaction(Faction faction, List<Character> characters)
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
