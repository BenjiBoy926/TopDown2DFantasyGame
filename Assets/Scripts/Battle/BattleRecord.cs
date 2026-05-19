using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class BattleRecord : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<BattleState> _states = new();

    public void RecordInitialState()
    {
        Debug.Log("Recorded initial state");
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