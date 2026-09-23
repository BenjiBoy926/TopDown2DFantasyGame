using System;
using UnityEngine;

public class PlayerActionTimer : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;
    private Battle _battle;
    private PlayerActionTimerUI _ui;
    private Action _undoAction;
    private Action _redoAction;
    private Action _pendingAction;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _ui = GetComponentInChildren<PlayerActionTimerUI>(true);
        _undoAction = () => _battle.Undo();
        _redoAction = () => _battle.Redo();
    }

    public void BeginUndo()
    {
        if (!_battle.IsUndoAvailable())
            return;

        Begin(_undoAction);
        _ui.BeginUndo(_duration);
    }

    public void CancelUndo()
    {
        Cancel(_undoAction);
    }

    public void BeginRedo()
    {
        if (!_battle.IsRedoAvailable())
            return;

        Begin(_redoAction);
        _ui.BeginRedo(_duration);
    }

    public void CancelRedo()
    {
        Cancel(_redoAction);
    }

    private void Begin(Action action)
    {
        _pendingAction = action;
        Invoke(nameof(Confirm), _duration);
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
        _ui.Cancel();
    }

    private void Confirm()
    {
        _pendingAction.Invoke();
        _ui.Confirm();
    }
}