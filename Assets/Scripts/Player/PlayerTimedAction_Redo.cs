using UnityEngine;

public class PlayerTimedAction_Redo : PlayerTimedAction
{
    public override string Label => "Redo";

    public override bool IsAvailable()
    {
        return base.IsAvailable() && Battle.IsRedoAvailable();
    }
    public override Coroutine Execute()
    {
        return Battle.Redo();
    }
}