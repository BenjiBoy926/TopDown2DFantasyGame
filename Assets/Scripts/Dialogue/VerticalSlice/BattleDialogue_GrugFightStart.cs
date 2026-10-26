using System.Collections;
using System.Collections.Generic;
using static UniqueCharacters;

namespace VerticalSlice
{
    public class BattleDialogue_GrugFightStart : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.MoveStart;

        private bool _haydenLinePlayed = false;
        private bool _genericLinePlayed = false;

        public override bool ShouldPlay(Character interactor, Character target)
        {
            Character playerCharacter = GetPlayerCharacter(interactor, target);
            bool isSomeoneGrug = interactor == Grug || target == Grug;
            return isSomeoneGrug && target != null && (ShouldPlayHaydenLine(playerCharacter) || !_genericLinePlayed);
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            Character playerCharacter = GetPlayerCharacter(interactor, target);
            if (ShouldPlayHaydenLine(playerCharacter))
            {
                _haydenLinePlayed = true;
                yield return Grug.Say("Puny human. You are lucky to be crushed under the yoke of Ha'all!");
                yield return Hayden.Say("Is this 'Ha'all' the same one that drove your kind to raze our towns and slaughter our women and children?!");
                yield return Grug.Say("Ha'all leads us to take the land that is ours by right!");
                yield return Hayden.Say("Not if I can help it!");
            }
            else if (!_genericLinePlayed)
            {
                _genericLinePlayed = true;
                yield return Grug.Say("Arrogant human.");
                yield return Grug.Say("We'll kill you and take your lands for our own! For Ha'all!");
            }
        }

        private bool ShouldPlayHaydenLine(Character other)
        {
            return other == Hayden && !_haydenLinePlayed;
        }

        private Character GetPlayerCharacter(Character interactor, Character target)
        {
            return IsPlayerCharacter(interactor) ? interactor : IsPlayerCharacter(target) ? target : null;
        }

        private bool IsPlayerCharacter(Character character)
        {
            return character && character.Faction == Battle.PlayerFaction;
        }
    }
}