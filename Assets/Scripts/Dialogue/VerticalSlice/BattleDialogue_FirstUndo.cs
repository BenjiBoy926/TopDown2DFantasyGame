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
            BattleState mostRecentUndoneState = Battle.GetState(Battle.CurrentStateIndex + 1);
            if (mostRecentUndoneState is BattleState_TurnChange)
            {
                
            }
            else
            {
                Character player = GetPlayerCharacter(mostRecentUndoneState);
                
            }
            yield break;
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