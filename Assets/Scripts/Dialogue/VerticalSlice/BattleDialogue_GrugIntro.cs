using System.Collections;
using UnityEngine;
using static UniqueCharacters;

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

    public override bool ShouldPlayOnTurnStart()
    {
        Squad grugSquad = Battle.GetSquad(Grug);
        bool isSquadAwake = grugSquad && (grugSquad.ShouldWakeUp() || grugSquad.IsAwake);
        bool areEnoughPlayersAlive = (Robin.IsAlive || Hayden.IsAlive) && (Gregory.IsAlive || Alfred.IsAlive);
        return !HasPlayed && Battle.CurrentFactionTurn != Battle.PlayerFaction && isSquadAwake && !Grug.IsDead;
    }

    protected override IEnumerator GetSequence(Character interactor, Character target)
    {
        yield return Battle.CameraGlide(Grug.transform);
        yield return Grug.Say("Tsk. They're a tenacious lot, I'll give them that.");
        yield return Grug.Say("No matter. They'll be no match for me.");
        yield return Grug.Say("Onward, worms! Show yourselves worthy of Ha'all!");

        yield return Battle.CameraGlide(GetAveragePlayerPosition());
        yield return new WaitForSeconds(1);
    }

    private Vector2 GetAveragePlayerPosition()
    {
        Vector2 position = Vector2.zero;
        for (int i = 0; i < MainCharacters.Count; i++)
        {
            position += MainCharacters[i].Position;
        }
        position /= MainCharacters.Count;
        return position;
    }
}