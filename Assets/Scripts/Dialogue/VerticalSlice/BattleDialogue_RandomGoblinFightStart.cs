using System.Collections;
using UnityEngine;

namespace VerticalSlice
{
    public class BattleDialogue_RandomGoblinFightStart : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.MoveStart;

        [SerializeField] private Character _goblin;

        public override bool ShouldPlay(Character interactor, Character target)
        {
            return !HasPlayed && (interactor == _goblin || target == _goblin) && interactor && target;
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            Character other = interactor == _goblin ? interactor : target;
            yield return _goblin.Say("Glory to Ha'all!");
            yield return Dialogue.FirstMatchingMainCharacterSay(other,
                "Ha'all? Who's Ha'all?",
                "Ha'all?",
                "What are you talking about?!",
                "Ha'all?");
            yield return _goblin.Say("You will see him soon enough!! GRAAHA!!");
        }
    }
}