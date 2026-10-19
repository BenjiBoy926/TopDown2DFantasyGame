using System.Collections;
using static UniqueCharacters;

namespace VerticalSlice
{
    public class BattleDialogue_FirstDeath : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.MoveEnd;

        public override bool ShouldPlay(Character interactor, Character target)
        {
            return !HasPlayed && (IsDeadPlayerCharacter(interactor) || IsDeadPlayerCharacter(target));
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            Character deadPlayer = IsDeadPlayerCharacter(interactor) ? interactor : target;

            string exclamation1 = $"{deadPlayer.Name}?";
            string exclamation2 = $"{deadPlayer.Name.ToUpper()}!! NO!!";

            yield return Dialogue.FirstLiveCharacterSay(
                new(Hayden, exclamation1),
                new(Alfred, exclamation1));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Hayden, exclamation2),
                new(Alfred, exclamation2));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Gregory, "Damn it..."),
                new(Robin, "Damn it..."));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Robin, "We have to turn back. They are too strong for us"),
                new(Alfred, "What do we do? We can't just leave him!"));

            if (Robin.IsAlive)
            {
                yield return Dialogue.FirstLiveCharacterSay(
                    new(Alfred, "We can't just leave him!"),
                    new(Gregory, "No. We cannot leave him."));

                yield return Dialogue.FirstLiveCharacterSay(
                    new(Gregory, "He's right. We must stick together. We must fight on!"),
                    new(Hayden, "He's right. We won't retreat."));

                if (Hayden.IsAlive)
                {
                    yield return Hayden.Say("Arkalux help us...");
                }
            }
            else
            {
                yield return Gregory.Say("Right. We must stick together. We must fight on!");
                yield return Hayden.Say("Arkalux help us...");
            }
        }

        private bool IsDeadPlayerCharacter(Character character)
        {
            return character && character.Faction == Battle.PlayerFaction && character.IsDead;
        }
    }
}