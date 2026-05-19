using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleHistory : MonoBehaviour
{
    private int LatestStateIndex => _states.Count - 1;

    [SerializeField, ReadOnly] private List<BattleState> _states = new();
    private int _currentStateIndex = 0;
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

        Debug.Log($"Undo state #{_currentStateIndex}");
        _currentStateIndex--;
    }

    public void Redo()
    {
        if (_currentStateIndex >= LatestStateIndex) return;

        Debug.Log($"Redo state #{_currentStateIndex}");
        _currentStateIndex++;
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
}