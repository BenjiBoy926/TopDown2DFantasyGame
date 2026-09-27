using UnityEngine;

public class PlayerTimedAction_Redo : PlayerTimedAction
{
    public override string Label => "Redo";

    public override bool IsQuickRepeatAvailable()
    {
        BattleState stateToRedo = Battle.GetRedoState();
        return stateToRedo is not BattleState_TurnChange;
    }
    public override bool IsAvailable()
    {
        return base.IsAvailable() && Battle.IsRedoAvailable();
    }
    public override Coroutine Execute()
    {
        return Battle.Redo();
    }
}