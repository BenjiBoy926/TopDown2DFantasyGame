using System.Collections;
using UnityEngine;
using static UniqueCharacters;

public class BattleDialogue_GrugIntro : BattleDialogueSequence
{
    [SerializeField] private Squad _grugSquad;

    public override bool ShouldPlayOnMoveStart(Character interactor, Character target)
    {
        return false;
    }

    public override bool ShouldPlayOnMoveFinish(Character interactor, Character target)
    {
        return false;
    }

    public override bool ShouldPlayOnTurnStart()
    {
        return Battle.CurrentFactionTurn != Battle.PlayerFaction && (_grugSquad.ShouldWakeUp() || _grugSquad.IsAwake) && !Grug.IsDead;
    }

    protected override IEnumerator GetSequence(Character interactor, Character target)
    {
        yield return Battle.CameraGlide(Grug.transform);
        yield return Grug.Say("Tsk. They're a tenacious lot, I'll give them that.");
        yield return Grug.Say("No matter. They'll be no match for me.");
        yield return Grug.Say("Onward, worms! Show yourselves worthy of Ha'all's blessing!");
    }
}