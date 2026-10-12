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

        string exclamation1 = $"{deadPlayer.Name}?";
        string exclamation2 = $"{deadPlayer.Name.ToUpper()}!! NO!!";
        if (!Hayden.IsDead)
        {
            yield return Hayden.Say(exclamation1);
            yield return Hayden.Say(exclamation2);
        }
        else
        {
            yield return Alfred.Say(exclamation1);
            yield return Alfred.Say(exclamation2);
        }
    }

    private bool IsDeadPlayerCharacter(Character character)
    {
        return character && character.Faction == Battle.PlayerFaction && character.IsDead;
    }
}