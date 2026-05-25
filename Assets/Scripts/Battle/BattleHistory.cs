using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleHistory : MonoBehaviour
{
    private int LatestStateIndex => _states.Count - 1;

    [SerializeField, ReadOnly] private List<BattleState> _states = new();
    [SerializeField, ReadOnly] private int _currentStateIndex = 0;
    private Battle _battle;
    private BattleUndoOverlay _overlay;

    private void Awake()
    {
        _battle = GetComponent<Battle>();
        _overlay = GetComponentInChildren<BattleUndoOverlay>();
    }

    public void RecordInitialState()
    {
        RecordTurnChange(_battle.AllCharacters, _battle.StartingFaction);
        _currentStateIndex = 0;
    }

    public void RecordTurnChange(IReadOnlyCollection<Character> characters, Faction faction)
    {
        BattleState state = new BattleState_TurnChange();
        state.SetCurrentTurn(faction);
        foreach (Character character in characters)
        {
            state.AddRecord(character);
        }
        InsertState(state);
    }

    public void Record(Character a, Character b)
    {
        BattleState state = new();
        state.SetCurrentTurn(_battle.CurrentFactionTurn);
        state.AddRecord(a);
        state.AddRecord(b);
        InsertState(state);
    }

    public void Record(Character character)
    {
        BattleState state = new();
        state.SetCurrentTurn(_battle.CurrentFactionTurn);
        state.AddRecord(character);
        InsertState(state);
    }

    public Coroutine Undo()
    {
        if (_currentStateIndex <= 0) return null;

        _currentStateIndex--;

        StopAllCoroutines();
        return StartCoroutine(ShowUndoSequence());
    }

    public Coroutine Redo()
    {
        if (_currentStateIndex >= LatestStateIndex) return null;

        _currentStateIndex++;

        StopAllCoroutines();
        return StartCoroutine(ShowRedoSequence());
    }

    private IEnumerator ShowUndoSequence()
    {
        yield return _overlay.FadeIn();

        int previousStateIndex = _currentStateIndex + 1;
        BattleState previousState = _states[previousStateIndex];
        for (int i = 0; i < previousState.RecordCount; i++)
        {
            CharacterRecord recordToUndo = previousState.GetRecord(i);
            CharacterRecord olderRecord = FindPreviousRecord(recordToUndo.Character, _currentStateIndex);
            yield return olderRecord.GetApplySequence();
        }
        SetCurrentTurn();

        yield return _overlay.FadeOut();
    }

    private IEnumerator ShowRedoSequence()
    {
        yield return _overlay.FadeIn();

        BattleState currentState = _states[_currentStateIndex];
        yield return currentState.GetAllApplySequences();
        SetCurrentTurn();

        yield return _overlay.FadeOut();
    }

    private void SetCurrentTurn()
    {
        BattleState state = _states[_currentStateIndex];
        _battle.StartTurn(state.CurrentTurn);
        if (state.IsTurnChange)
        {
            _battle.PlayTurnChangeAnimation();
        }
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