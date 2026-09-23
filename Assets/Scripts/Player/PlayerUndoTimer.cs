using System;
using UnityEngine;

public class PlayerUndoTimer : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;
    private Battle _battle;
    private Action _undoAction;
    private Action _redoAction;
    private Action _pendingAction;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _undoAction = () => _battle.Undo();
        _redoAction = () => _battle.Redo();
    }

    public void BeginUndo()
    {
        Begin(_undoAction);
    }

    public void CancelUndo()
    {
        Cancel(_undoAction);
    }

    public void BeginRedo()
    {
        Begin(_redoAction);
    }

    public void CancelRedo()
    {
        Cancel(_redoAction);
    }

    private void Begin(Action action)
    {
        _pendingAction = action;
        Invoke(nameof(Trigger), _duration);
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
    }

    private void Trigger()
    {
        _pendingAction.Invoke();
    }
}