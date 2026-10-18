using System.Collections;
using static UniqueCharacters;

namespace VerticalSlice
{
    public class BattleDialogue_FirstReviveStart : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.MoveStart;

        public override bool ShouldPlay(Character interactor, Character target)
        {
            return !HasPlayed && target && interactor.Faction == target.Faction && target.IsDead;
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            return Dialogue.FirstMatchingMainCharacterSay(interactor,
                $"Huh? The Ember... it's leading me over to {target.Name}",
                $"Strange. The Ember is leading me over to {target.Name}?",
                $"What the heck? Why is the Ember leading me over to {target.Name}?",
                $"Hm? The Ember. It's leading me over to {target.Name}.");
        }
    }
}