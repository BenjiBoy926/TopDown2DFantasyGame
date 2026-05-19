using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleHistory : MonoBehaviour
{
    private int LatestStateIndex => _states.Count - 1;

    [SerializeField, ReadOnly] private List<BattleState> _states = new();
    [SerializeField, ReadOnly] private int _currentStateIndex = 0;
    private Battle _battle;

    private void Awake()
    {
        _battle = GetComponent<Battle>();
    }

    public void RecordInitialState()
    {
        BattleState initialState = new();
        foreach (Character character in _battle.AllCharacters)
        {
            initialState.AddRecord(character);
        }
        _states.Add(initialState);
    }

    public void Record(Character a, Character b)
    {
        BattleState state = new();
        state.AddRecord(a);
        state.AddRecord(b);
        InsertState(state);
    }

    public void Record(Character character)
    {
        BattleState state = new();
        state.AddRecord(character);
        InsertState(state);
    }

    public void Undo()
    {
        if (_currentStateIndex <= 0) return;

        _currentStateIndex--;

        BattleState previousState = _states[_currentStateIndex + 1];
        for (int i = 0; i < previousState.RecordCount; i++)
        {
            CharacterRecord recordToUndo = previousState.GetRecord(i);
            CharacterRecord olderRecord = FindPreviousRecord(recordToUndo.Character, _currentStateIndex);
            olderRecord.Apply();
        }
    }

    public void Redo()
    {
        if (_currentStateIndex >= LatestStateIndex) return;

        _currentStateIndex++;

        BattleState currentState = _states[_currentStateIndex];
        currentState.ApplyAll();
    }

    private void InsertState(BattleState state)
    {
        if (_currentStateIndex < LatestStateIndex)
        {
            _states.RemoveRange(_currentStateIndex + 1, LatestStateIndex - _currentStateIndex);
        }
        _states.Add(state);
        _currentStateIndex++;
    }

    private CharacterRecord FindPreviousRecord(Character character, int startIndex)
    {
        for (int i = startIndex; i >= 0; i--)
        {
            BattleState state = _states[i];
            if (state.TryGetRecord(character, out var record))
            {
                return record;
            }
        }
        throw new System.Exception($"Expected to find a record for {character} at or before index {startIndex}, " +
            $"but no such record could be found. Are you sure that the initial state of the character was recorded " +
            $"when they first entered the battle?");
    }
}