using UnityEngine;

public class PlayerTimedAction_EndTurn : PlayerTimedAction
{
    public override string Label => "End Turn";

    public override Coroutine Execute()
    {
        Battle.StartNextTurn();
        return null;
    }
}