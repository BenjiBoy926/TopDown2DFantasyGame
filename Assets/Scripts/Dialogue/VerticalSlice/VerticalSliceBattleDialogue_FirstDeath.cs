using System.Collections;
using static UniqueCharacters;

public class VerticalSliceDialogue_FirstDeath : BattleDialogueSequence
{
    public override bool ShouldPlay(Character interactor, Character target)
    {
        return !HasPlayed && (IsDeadPlayerCharacter(interactor) || IsDeadPlayerCharacter(target));
    }

    public override IEnumerator Play(Character interactor, Character target)
    {
        yield return base.Play(interactor, target);

        Character deadPlayer = IsDeadPlayerCharacter(interactor) ? interactor : target;
        string initialExclamation = $"{deadPlayer.Name}? {deadPlayer.Name.ToUpper()}!! NO!!";
        if (!Hayden.IsDead)
        {
            yield return Hayden.Say(initialExclamation);
        }
        else
        {
            yield return Alfred.Say(initialExclamation);
        }
    }

    private bool IsDeadPlayerCharacter(Character character)
    {
        return character && character.Faction == Battle.PlayerFaction && character.IsDead;
    }
}