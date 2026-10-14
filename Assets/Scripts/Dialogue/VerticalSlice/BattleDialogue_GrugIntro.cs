using System.Collections;

public class BattleDialogue_GrugIntro : BattleDialogueSequence
{
    public override bool ShouldPlayOnMoveStart(Character interactor, Character target)
    {
        return false;
    }

    public override bool ShouldPlayOnMoveFinish(Character interactor, Character target)
    {
        return false;
    }

    protected override IEnumerator GetSequence(Character interactor, Character target)
    {
        yield return null;
    }
}