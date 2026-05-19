using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleRecord : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<BattleState> _states = new();
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
        _states.Add(state);
    }

    public void Record(Character character)
    {
        BattleState state = new();
        state.AddRecord(character);
        _states.Add(state);
    }

    public void Undo()
    {
        Debug.Log("Undo!");
    }

    public void Redo()
    {
        Debug.Log("Redo!");
    }
}