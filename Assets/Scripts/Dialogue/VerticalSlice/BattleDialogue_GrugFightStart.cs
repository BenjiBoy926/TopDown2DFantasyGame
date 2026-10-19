using System.Collections;
using System.Collections.Generic;
using static UniqueCharacters;

namespace VerticalSlice
{
    public class BattleDialogue_GrugFightStart : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.MoveStart;

        private readonly HashSet<Character> _foughtCharacters = new();

        public override bool ShouldPlay(Character interactor, Character target)
        {
            Character playerCharacter = GetPlayerCharacter(interactor, target);
            bool isSomeoneGrug = interactor == Grug || target == Grug;
            return isSomeoneGrug && playerCharacter && !_foughtCharacters.Contains(playerCharacter);
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            Character playerCharacter = GetPlayerCharacter(interactor, target);
            _foughtCharacters.Add(playerCharacter);

            if (playerCharacter == Hayden)
            {
                yield return Grug.Say("Puny human. You are lucky to be crushed under the yoke of Ha'all!");
                yield return Hayden.Say("Is this 'Ha'all' the same one that drove your kind to raze our towns and slaughter our women and children?!");
                yield return Grug.Say("Ha'all leads us to take the land that is ours by right!");
                yield return Hayden.Say("Not if I can help it!");
            }
            else if (playerCharacter == Gregory)
            {
                yield return Grug.Say("Your armor is thick, stout one.");
                yield return Grug.Say("But you will not endure the crushing might of Ha'all!");
                yield return Gregory.Say("Not alone. But together we will outlast you and every demon horde beyond you!");
            }
            else if (playerCharacter == Alfred)
            {
                yield return Alfred.Say("Eat spear, filthy orc!");
                yield return Grug.Say("Grah. I do not fear that puny stick of yours.");
                yield return Grug.Say("Come and try me!");
            }
            else // Robin
            {
                yield return Grug.Say("Forward! For Ha'all!");
                yield return Robin.Say("Stupid brute. We'll outsmart you and move on without difficulty.");
                yield return Grug.Say("Graow! You'll pay for your insolence!");
                yield return Robin.Say("Try me.");
            }
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