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
            // should play if a character is a player and they haven't fought Grug yet
            return isSomeoneGrug && playerCharacter && !_foughtCharacters.Contains(playerCharacter);
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            Character playerCharacter = GetPlayerCharacter(interactor, target);
            _foughtCharacters.Add(playerCharacter);

            if (playerCharacter == Hayden)
            {

            }
            else if (playerCharacter == Gregory)
            {

            }
            else if (playerCharacter == Alfred)
            {

            }
            else // Robin
            {

            }
            yield break;
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