using System.Collections;

namespace VerticalSlice
{
    public class BattleDialogue_FirstUndo : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.UndoRedoEnd;

        public override bool ShouldPlay(Character interactor, Character target)
        {
            return !HasPlayed;
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            // If someone was dead and is no longer dead, then they should talk
            // Else if someone moved to a different location, then they should talk
            // Else if you undid a turn change, then do a prescripted dialogue where characters noticed the goblins moving
            // Else? (no one died, no one moved, no turn change, basically the character waiting in the same space and undid, then no comment? Or maybe you do do a comment?)
            
            BattleState mostRecentUndoneState = Battle.GetState(Battle.CurrentStateIndex + 1);
            if (mostRecentUndoneState is BattleState_TurnChange)
            {
                
            }
            else
            {
                Character player = GetPlayerCharacter(mostRecentUndoneState);
                yield return Dialogue.FirstMatchingMainCharacterSay(player,
                    "Woah! What was that?",
                    "Hm? What was that?",
                    "Woah! Woah! What? What the heck?!",
                    "...That was strange...");
                yield return Dialogue.FirstMatchingMainCharacterSay(player,
                    "I was just over there, but now... I'm back where I started?",
                    "I've come back where I started moments ago...",
                    "Deja-vu! Wasn't I JUST here?!",
                    "It's like time just turned back...");
            }
        }

        private Character GetPlayerCharacter(BattleState state)
        {
            for (int i = 0; i < state.RecordCount; i++)
            {
                CharacterRecord record = state.GetRecord(i);
                if (record.Character.Faction == Battle.PlayerFaction)
                {
                    return record.Character;
                }
            }
            return null;
        }
    }
}