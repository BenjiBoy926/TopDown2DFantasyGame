using System;
using UnityEngine;

[RequireComponent(typeof(PlayerTimedAction_Undo))]
[RequireComponent(typeof(PlayerTimedAction_Redo))]
[RequireComponent(typeof(PlayerTimedAction_EndTurn))]
public class PlayerActionTimer : MonoBehaviour
{
    private Battle _battle;
    private PlayerActionTimerUI _ui;
    private PlayerTimedAction_Undo _undoAction;
    private PlayerTimedAction_Redo _redoAction;
    private PlayerTimedAction_EndTurn _endTurnAction;
    private PlayerTimedAction _pendingAction;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _ui = GetComponentInChildren<PlayerActionTimerUI>(true);
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
        if (!action.IsAvailable())
            return;

        _pendingAction = action;
        Invoke(nameof(Confirm), action.Duration);
        _ui.Begin(_pendingAction);
    }

    private void Cancel(PlayerTimedAction action)
    {
        if (_pendingAction == action)
        {
            Cancel();
        }
    }

    private void Cancel()
    {
        CancelInvoke();
        _ui.CancelAnimation();
    }

    private void Confirm()
    {
        // TODO: wait on the coroutine returned to see if we need to rapidly repeat the same action
        _pendingAction.Execute();
        _ui.PlayConfirmAnimation();
    }
}