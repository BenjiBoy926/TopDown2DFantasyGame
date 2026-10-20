using System.Collections;

namespace VerticalSlice
{
    public class BattleDialogue_AllEnemiesDefeated : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.MoveEnd;

        public override bool ShouldPlay(Character interactor, Character target)
        {
            return !HasPlayed && Battle.AreAllEnemiesDead() && !Battle.AreAllPlayersAlive();
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            Character player = IsPlayerCharacter(interactor) ? interactor : target;

            string[] line1 = new string[]
            {
                "Ha... Ha... that was hard-fought...",
                "Ha... Ha... that was a tough fight...",
                "Ha... Ha... finally! Tough... opponents...",
                "Ha... Ha... so much trouble to follow a stupid blue fire..."
            };
            string[] line2 = new string[]
            {
                "But in the end... we prevailed! Ha...",
                "But working together... we finally pulled through... Ha...",
                "But clearly... we're still tougher! Ha...",
                "Whatever. At least we finally outsmarted them... Ha..."
            };
            string[] line3 = new string[]
            {
                "We'd better get patched up. Thank Arkalux for the Ember's healing or we'd be done for!",
                "We'd better get patched up and move out before reinforcements come.",
                "We'd better get patched up. Let's get out of here!",
                "We'd better get patched up and move one."
            };

            if (player.IsAlive)
            {
                yield return Dialogue.FirstMatchingMainCharacterSay(player, line1);
                yield return Dialogue.FirstMatchingMainCharacterSay(player, line2);
                yield return Dialogue.FirstMatchingMainCharacterSay(player, line3);
            }
            else
            {
                yield return Dialogue.FirstLiveMainCharacterSay(line1);
                yield return Dialogue.FirstLiveMainCharacterSay(line2);
                yield return Dialogue.FirstLiveMainCharacterSay(line3);
            }
        }

        private bool IsPlayerCharacter(Character character)
        {
            return character && character.Faction == Battle.PlayerFaction;
        }
    }
}