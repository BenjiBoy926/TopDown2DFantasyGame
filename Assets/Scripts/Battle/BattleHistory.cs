using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleHistory : MonoBehaviour
{
    private int LatestStateIndex => _states.Count - 1;
    public static bool IsAnySequencePlaying => _playingHistories.Count > 0;

    [SerializeField, ReadOnly] private List<BattleState> _states = new();
    [SerializeField, ReadOnly] private int _currentStateIndex = 0;
    private Battle _battle;
    private BattleUndoOverlay _overlay;
    private static readonly HashSet<BattleHistory> _playingHistories = new();

    private void Awake()
    {
        _battle = GetComponent<Battle>();
        _overlay = GetComponentInChildren<BattleUndoOverlay>();
    }

    public void RecordInitialState()
    {
        RecordTurnChange(_battle.AllCharacters, _battle.PlayerFaction);
        _currentStateIndex = 0;
    }

    public void RecordTurnChange(IReadOnlyCollection<Character> characters, Faction faction)
    {
        BattleState state = new BattleState_TurnChange();
        state.SetCurrentTurn(faction);
        foreach (Character character in characters)
        {
            if (character.IsInBattle)
            {
                state.AddRecord(character);
            }
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
        StopAllCoroutines();
        return _currentStateIndex > 0 ? StartCoroutine(UndoToLastPlayerMovePoint()) : null;
    }

    public Coroutine Redo()
    {
        StopAllCoroutines();
        return _currentStateIndex < LatestStateIndex ? StartCoroutine(RedoToLastPlayerMovePoint()) : null;
    }

    private IEnumerator UndoToLastPlayerMovePoint()
    {
        yield return SequenceStart();

        do
        {
            _currentStateIndex--;
            yield return UndoOnceSequence();
        }
        while (!CanPlayerMove() && _currentStateIndex > 0);

        yield return SequenceEnd();
    }

    private IEnumerator RedoToLastPlayerMovePoint()
    {
        yield return SequenceStart();

        do
        {
            _currentStateIndex++;
            yield return RedoOnceSequence();
        }
        while (!CanPlayerMove() && _currentStateIndex < LatestStateIndex);

        yield return SequenceEnd();
    }

    private IEnumerator UndoOnceSequence()
    {
        int previousStateIndex = _currentStateIndex + 1;
        BattleState previousState = _states[previousStateIndex];
        for (int i = 0; i < previousState.RecordCount; i++)
        {
            CharacterRecord recordToUndo = previousState.GetRecord(i);
            CharacterRecord olderRecord = FindPreviousRecord(recordToUndo.Character, _currentStateIndex);
            yield return olderRecord.GetApplySequence();
        }
        SetCurrentTurn();
    }

    private IEnumerator RedoOnceSequence()
    {
        BattleState currentState = _states[_currentStateIndex];
        yield return currentState.GetAllApplySequences();
        SetCurrentTurn();
    }

    private bool CanPlayerMove()
    {
        return _battle.CurrentFactionTurn == _battle.PlayerFaction && _battle.CountMoveableCharacters(_battle.PlayerFaction) > 0;
    }

    private IEnumerator SequenceStart()
    {
        _playingHistories.Add(this);
        yield return _overlay.FadeIn();
    }

    private IEnumerator SequenceEnd()
    {
        yield return _overlay.FadeOut();
        _playingHistories.Remove(this);
    }   

    private void SetCurrentTurn()
    {
        BattleState state = _states[_currentStateIndex];
        _battle.StartTurn(state.CurrentTurn);
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