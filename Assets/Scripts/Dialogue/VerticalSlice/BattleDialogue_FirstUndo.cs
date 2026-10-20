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
            yield break;
        }
    }
}