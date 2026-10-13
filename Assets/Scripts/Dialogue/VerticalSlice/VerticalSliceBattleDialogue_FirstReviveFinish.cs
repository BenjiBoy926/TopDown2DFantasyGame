using System.Collections;
using UnityEngine;

public class VerticalSliceBattleDialogue_FirstReviveFinish : BattleDialogueSequence
{
    public override bool ShouldPlayOnMoveStart(Character interactor, Character target)
    {
        return false;
    }

    public override bool ShouldPlayOnMoveFinish(Character interactor, Character target)
    {
        return target && interactor.Faction == target.Faction && target.GetLastRecordedState().Health <= 0;
    }

    public override IEnumerator GetSequence(Character interactor, Character target)
    {
        Debug.Log("Revive finish!");
        yield return null;
    }
}