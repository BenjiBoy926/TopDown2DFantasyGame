using System.Collections;
using UnityEngine;

namespace VerticalSlice
{
    [RequireComponent(typeof(Character))]
    public class BattleDialogue_RandomGoblinFightEnd : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.MoveEnd;

        private Character _goblin;
        private bool _hasPlayedKillLine;
        private bool _hasPlayedDeathLine;

        private void Awake()
        {
            _goblin = GetComponent<Character>();
        }

        public override bool ShouldPlay(Character interactor, Character target)
        {
            bool isEitherCharacterTheGoblin = interactor == _goblin || target == _goblin;
            bool hasPlayedBothLines = _hasPlayedDeathLine && _hasPlayedKillLine;
            return isEitherCharacterTheGoblin && interactor && target && !hasPlayedBothLines;
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            Character other = interactor == _goblin ? target : interactor;
            if (_goblin.IsDead)
            {
                yield return PlayDeadLine(other);
            }
            else if (other.IsDead)
            {
                yield return PlayKillLine();
            }
        }

        private IEnumerator PlayDeadLine(Character other)
        {
            if (_hasPlayedDeathLine)
                yield break;

            _hasPlayedDeathLine = true;
            yield return _goblin.Say("...glory to... Ha'all... urk...");
            if (other.IsAlive)
            {
                yield return Dialogue.FirstMatchingMainCharacterSay(other,
                    "...Ha'all? What could that mean...",
                    "What could 'Ha'all' mean...",
                    "What is this guy even talking about?!",
                    "What's this nonesense about 'Ha'all'?");
            }
        }

        private IEnumerator PlayKillLine()
        {
            if (_hasPlayedKillLine)
                yield break;

            _hasPlayedKillLine = true;
            yield return _goblin.Say("Gahaa!!");
            yield return _goblin.Say("Another victory for Ha'all's chosen people!!");
        }
    }
}