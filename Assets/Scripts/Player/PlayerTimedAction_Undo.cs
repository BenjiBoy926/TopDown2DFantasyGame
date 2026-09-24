using UnityEngine;

public class PlayerTimedAction_Undo : PlayerTimedAction
{
    public override string Label => "Undo";

    public override bool IsAvailable()
    {
        return base.IsAvailable() && Battle.IsUndoAvailable();
    }
    public override Coroutine Execute()
    {
        return Battle.Undo();
    }
}