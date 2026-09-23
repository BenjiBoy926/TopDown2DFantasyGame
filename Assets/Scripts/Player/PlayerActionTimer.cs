using System;
using UnityEngine;

[RequireComponent(typeof(PlayerTimedAction_Undo))]
[RequireComponent(typeof(PlayerTimedAction_Redo))]
[RequireComponent(typeof(PlayerTimedAction_EndTurn))]
public class PlayerActionTimer : MonoBehaviour
{
    private PlayerTimedAction_Undo _undoAction;
    private PlayerTimedAction_Redo _redoAction;
    private PlayerTimedAction_EndTurn _endTurnAction;
    private PlayerTimedAction _currentlyRunningAction;

    private void Awake()
    {
        _undoAction = GetComponent<PlayerTimedAction_Undo>();
        _redoAction = GetComponent<PlayerTimedAction_Redo>();
        _endTurnAction = GetComponent<PlayerTimedAction_EndTurn>();
    }

    public void BeginUndo()
    {
        Begin(_undoAction);
    }

    public void BeginRedo()
    {
        Begin(_redoAction);
    }

    public void BeginEndTurn()
    {
        Begin(_endTurnAction);
    }

    public void CancelUndo()
    {
        Cancel(_undoAction);
    }

    public void CancelRedo()
    {
        Cancel(_redoAction);
    }

    public void CancelEndTurn()
    {
        Cancel(_endTurnAction);
    }

    private void Begin(PlayerTimedAction action)
    {
        if (action.Begin())
        {
            _currentlyRunningAction = action;
        }
    }

    private void Cancel(PlayerTimedAction action)
    {
        if (_currentlyRunningAction == action)
        {
            action.Cancel();
            _currentlyRunningAction = null;
        }
    }
}