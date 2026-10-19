using System.Collections;
using static UniqueCharacters;

public class BattleDialogue_GrugDeath : BattleDialogueSequence
{
    public override TriggerType Trigger => TriggerType.MoveEnd;

    public override bool ShouldPlay(Character interactor, Character target)
    {
        return !HasPlayed && (interactor == Grug || target == Grug) && Grug.IsDead;
    }

    protected override IEnumerator GetSequence(Character interactor, Character target)
    {
        yield return Grug.Say("Gah...");
        yield return Grug.Say("I will dine with Ha'all...");
        yield return Grug.Say("in... paradise... urk...");
    }
}