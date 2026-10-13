using System.Collections;
using static UniqueCharacters;

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

    public override IEnumerator GetSequence(Character interactor, Character target)
    {
        if (interactor == Hayden)
        {
            yield return Hayden.Say($"Huh? The Ember... it's leading me over to {target.Name}");
        }
        else if (interactor == Gregory)
        {
            yield return Gregory.Say($"Strange. The Ember is leading me over to {target.Name}?");
        }
        else if (interactor == Alfred)
        {
            yield return Alfred.Say($"What the heck? Why is the Ember leading me over to {target.Name}?");
        }
        else if (interactor == Robin)
        {
            yield return Robin.Say($"Hm? The Ember. It's leading me over to {target.Name}.");
        }
    }
}