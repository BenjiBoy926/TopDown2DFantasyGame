using System.Collections;
using UnityEngine;

public class VerticalSliceBattleDialogue_FirstReviveStart : BattleDialogueSequence
{
    public override bool ShouldPlayOnMoveStart(Character interactor, Character target)
    {
        return !HasPlayed && target && interactor.Faction == target.Faction && target.IsDead;
    }
    public override bool ShouldPlayOnMoveFinish(Character interactor, Character target)
    {
        return false;
    }

    public override IEnumerator Play(Character interactor, Character target)
    {
        yield return base.Play(interactor, target);
        Debug.Log("First revive is starting");
    }
}