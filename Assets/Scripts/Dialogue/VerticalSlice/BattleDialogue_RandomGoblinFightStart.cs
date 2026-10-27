using System.Collections;
using UnityEngine;

namespace VerticalSlice
{
    [RequireComponent(typeof(Character))]
    public class BattleDialogue_RandomGoblinFightStart : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.MoveStart;

        private Character _goblin;

        private void Awake()
        {
            _goblin = GetComponent<Character>();
        }

        public override bool ShouldPlay(Character interactor, Character target)
        {
            return !HasPlayed && (interactor == _goblin || target == _goblin) && interactor && target;
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            Character other = interactor == _goblin ? target : interactor;
            yield return _goblin.Say("Glory to Ha'all!");
            yield return Dialogue.FirstMatchingMainCharacterSay(other,
                "Ha'all? Who's Ha'all?",
                "Ha'all?",
                "What are you talking about?!",
                "Ha'all?");
            yield return _goblin.Say("You'll see him soon enough!! GRAAHA!!");
        }
    }
}