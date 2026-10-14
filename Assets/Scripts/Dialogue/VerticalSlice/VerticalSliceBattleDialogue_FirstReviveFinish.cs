using System.Collections;
using UnityEngine;
using static UniqueCharacters;

public class VerticalSliceBattleDialogue_FirstReviveFinish : BattleDialogueSequence
{
    public override bool ShouldPlayOnMoveStart(Character interactor, Character target)
    {
        return false;
    }

    public override bool ShouldPlayOnMoveFinish(Character interactor, Character target)
    {
        return !HasPlayed && target && interactor.Faction == target.Faction && target.GetLastRecordedState().Health <= 0;
    }

    protected override IEnumerator GetSequence(Character interactor, Character target)
    {
        yield return Dialogue.MainCharacterSay(interactor,
            "Hey! Are you okay?",
            "Are you alright?",
            "Woah! Are you okay?!",
            "Oh? You're up? Are you alright?");

        yield return Dialogue.MainCharacterSay(target,
            "Yeah, I think so. I'm pretty exhausted, but I'm not in any pain.",
            "...I think so. I feel exhausted, but I'm not in any pain.",
            "I feel amazing! Although, *yawn, I could really use a nap...",
            "...Yes. I'm tired but my wounds are all healed.");

        if (target == Alfred)
        {
            yield return interactor.Say("Alfred, Focus!");
            yield return target.Say("Right!");
        }

        yield return Dialogue.MainCharacterSay(target,
            "Was this the Ember's doing? What happened?",
            "What happened?",
            "Sooooo, what happened?!",
            "What happened?");

        yield return Dialogue.MainCharacterSay(interactor,
            "It was the Ember's doing. Somehow it used me to heal you.",
            "I'm not sure. Somehow the Ember used me to heal you.",
            "Beats me! I felt the Ember's energy coursing through me, and suddenly you were okay!",
            "I think the Ember used me to heal you.");

        yield return Dialogue.MainCharacterSay(target,
            "So it WAS the Ember!",
            "The Ember? Remarkable...",
            "So it was the Ember, huh?",
            "The Ember? Well... that should help to even the odds.");

        yield return Dialogue.MainCharacterSay(target,
            "We can win now, I'm sure of it! Arkalux will uphold us with his Ember!",
            "Using this power, we can work together and cut down this horde!",
            "We can use this power to break through their ranks! Let's go!",
            "Let's use this power to overcome these monsters.");
    }
}