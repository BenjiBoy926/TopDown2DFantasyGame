using System.Collections;
using static UniqueCharacters;

namespace VerticalSlice
{
    public class BattleDialogue_GrugKill : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.MoveEnd;

        public override bool ShouldPlay(Character interactor, Character target)
        {
            bool isSomeoneGrug = Grug == interactor || Grug == target;
            bool isSomeoneDeadPlayerCharacter = IsDeadPlayer(interactor) || IsDeadPlayer(target);
            return !HasPlayed && Grug.IsAlive && isSomeoneGrug && isSomeoneDeadPlayerCharacter;
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            yield return Grug.Say("Grahahaaa!");
            yield return Grug.Say("Pathetic! You deserve only subjugation under Ha'all and his hordes!");
        }

        private bool IsDeadPlayer(Character character)
        {
            return character && character.Faction == Battle.PlayerFaction && character.IsDead;
        }
    }
}