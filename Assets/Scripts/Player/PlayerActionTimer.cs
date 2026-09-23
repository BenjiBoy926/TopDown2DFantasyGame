using System;
using UnityEngine;

public class PlayerActionTimer : MonoBehaviour
{
    private abstract class TimedAction
    {
        public float Duration;
        
        public abstract Coroutine Execute(Battle battle);
    }

    private class TimedAction_Undo : TimedAction
    {
        public override Coroutine Execute(Battle battle)
        {
            return battle.Undo();
        }
    }

    private class TimedAction_Redo : TimedAction
    {
        public override Coroutine Execute(Battle battle)
        {
            return battle.Redo();
        }
    }

    private class TimedAction_EndTurn : TimedAction
    {
        public override Coroutine Execute(Battle battle)
        {
            battle.StartNextTurn();
            return null;
        }
    }

    [SerializeField] private float _undoRedoDuration = 0.5f;
    [SerializeField] private float _endTurnDuration = 1.5f;
    private Battle _battle;
    private PlayerActionTimerUI _ui;
    private TimedAction_Undo _undoAction;
    private TimedAction_Redo _redoAction;
    private TimedAction_EndTurn _endTurnAction;
    private TimedAction _pendingAction;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _ui = GetComponentInChildren<PlayerActionTimerUI>(true);
        _undoAction = new TimedAction_Undo() { Duration = _undoRedoDuration };
        _redoAction = new TimedAction_Redo() { Duration = _undoRedoDuration };
        _endTurnAction = new TimedAction_EndTurn() { Duration = _endTurnDuration };
    }

    public void BeginUndo()
    {
        if (!_battle.IsUndoAvailable())
            return;

        Begin(_undoAction);
        _ui.BeginUndo(_undoRedoDuration);
    }

    public void BeginRedo()
    {
        if (!_battle.IsRedoAvailable())
            return;

        Begin(_redoAction);
        _ui.BeginRedo(_undoRedoDuration);
    }

    public void BeginEndTurn()
    {
        Begin(_endTurnAction);
        _ui.BeginEndTurn(_endTurnDuration);
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

    private void Begin(TimedAction action)
    {
        _pendingAction = action;
        Invoke(nameof(Confirm), action.Duration);
    }

    private void Cancel(TimedAction action)
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
        _pendingAction.Execute(_battle);
        _ui.PlayConfirmAnimation();
    }
}