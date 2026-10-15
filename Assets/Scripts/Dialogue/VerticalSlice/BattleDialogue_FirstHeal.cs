using System.Collections;

namespace VerticalSlice
{
    public class BattleDialogue_FirstHeal : BattleDialogueSequence
    {
        public override bool ShouldPlayOnMoveStart(Character interactor, Character target)
        {
            return false;
        }

        public override bool ShouldPlayOnMoveFinish(Character interactor, Character target)
        {
            return !HasPlayed && target && interactor.Faction == target.Faction && target.GetLastRecordedState().Health > 0;
        }

        public override bool ShouldPlayOnTurnStart()
        {
            return false;
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            yield return Dialogue.FirstMatchingMainCharacterSay(target,
                "Wow, I feel great! What just happened?",
                "Huh? That was refreshing. What was that?",
                "Woah! Woah! What? I feel like new! What just happened?",
                "Hm? My wounds are all healed. What was that?");

            yield return Dialogue.FirstMatchingMainCharacterSay(interactor,
                "I think... I think the Ember just used me to heal you!",
                "Not sure. The Ember led me to you and suddenly energy flowed through me into you.",
                "No idea! I was just following the Ember, then suddenly I felt energy race through me and into you.",
                "...It seems like the Ember used me to heal you.");

            yield return Dialogue.FirstMatchingMainCharacterSay(target,
                "What an incredible power! We can use this to beat back the goblins!",
                "Working together with this power, we should be able to get the upper hand.",
                "AMAZING! Now we can charge ahead without worrying so much about damage!",
                "This should give us a tactical advantage. We can use this power to beat the goblins and get out of here.");
        }
    }
}