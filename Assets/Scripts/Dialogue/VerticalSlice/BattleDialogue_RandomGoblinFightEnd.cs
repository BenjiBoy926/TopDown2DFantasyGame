using System.Collections;

namespace VerticalSlice
{
    public class BattleDialogue_RandomGoblinFightEnd : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.MoveEnd;

        public override bool ShouldPlay(Character interactor, Character target)
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            throw new System.NotImplementedException();
        }
    }
}