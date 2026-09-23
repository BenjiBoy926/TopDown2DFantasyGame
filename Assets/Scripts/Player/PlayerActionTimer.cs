using System;
using UnityEngine;

public class PlayerActionTimer : MonoBehaviour
{
    [SerializeField] private float _undoRedoDuration = 0.5f;
    [SerializeField] private float _endTurnDuration = 1.5f;
    private Battle _battle;
    private PlayerActionTimerUI _ui;
    private Action _undoAction;
    private Action _redoAction;
    private Action _endTurnAction;
    private Action _pendingAction;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _ui = GetComponentInChildren<PlayerActionTimerUI>(true);
        _undoAction = () => _battle.Undo();
        _redoAction = () => _battle.Redo();
        _endTurnAction = _battle.StartNextTurn;
    }

    public void BeginUndo()
    {
        if (!_battle.IsUndoAvailable())
            return;

        Begin(_undoAction, _undoRedoDuration);
        _ui.BeginUndo(_undoRedoDuration);
    }

    public void BeginRedo()
    {
        if (!_battle.IsRedoAvailable())
            return;

        Begin(_redoAction, _undoRedoDuration);
        _ui.BeginRedo(_undoRedoDuration);
    }

    public void BeginEndTurn()
    {
        Begin(_endTurnAction, _endTurnDuration);
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

    private void Begin(Action action, float duration)
    {
        _pendingAction = action;
        Invoke(nameof(Confirm), duration);
    }

    private void Cancel(Action action)
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
        _pendingAction.Invoke();
        _ui.PlayConfirmAnimation();
    }
}