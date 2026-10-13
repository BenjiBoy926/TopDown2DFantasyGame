using System.Collections;
using static UniqueCharacters;

public class VerticalSliceDialogue_FirstDeath : BattleDialogueSequence
{
    public override bool ShouldPlayOnMoveStart(Character interactor, Character target)
    {
        return false;
    }

    public override bool ShouldPlayOnMoveFinish(Character interactor, Character target)
    {
        return !HasPlayed && (IsDeadPlayerCharacter(interactor) || IsDeadPlayerCharacter(target));
    }

    public override IEnumerator GetSequence(Character interactor, Character target)
    {
        Character deadPlayer = IsDeadPlayerCharacter(interactor) ? interactor : target;

        string exclamation1 = $"{deadPlayer.Name}?";
        string exclamation2 = $"{deadPlayer.Name.ToUpper()}!! NO!!";

        if (Hayden.IsDead)
        {
            yield return Alfred.Say(exclamation1);
            yield return Alfred.Say(exclamation2);
            yield return Gregory.Say("Damn it...");
            yield return Robin.Say("We have to turn back. They are too strong for us");
            yield return Alfred.Say("We can't just leave him!");
            yield return Gregory.Say("He's right. We must stick together. We must fight on!");
        }
        else if (Gregory.IsDead)
        {
            yield return Hayden.Say(exclamation1);
            yield return Hayden.Say(exclamation2);
            yield return Robin.Say("Damn it...");
            yield return Robin.Say("We have to turn back. They are too strong for us");
            yield return Alfred.Say("We can't just leave him!");
            yield return Hayden.Say("He's right. We won't retreat.");
            yield return Hayden.Say("Arkalux help us...");
        }
        else if (Alfred.IsDead)
        {
            yield return Hayden.Say(exclamation1);
            yield return Hayden.Say(exclamation2);
            yield return Gregory.Say("Damn it...");
            yield return Robin.Say("We have to turn back. They are too strong for us");
            yield return Gregory.Say("No. We cannot leave him.");
            yield return Gregory.Say("We must stick together. We must fight on!");
            yield return Hayden.Say("Arkalux help us...");
        }
        else
        {
            yield return Hayden.Say(exclamation1);
            yield return Hayden.Say(exclamation2);
            yield return Gregory.Say("Damn it...");
            yield return Alfred.Say("What do we do? We can't just leave him!");
            yield return Gregory.Say("Right. We must stick together. We must fight on!");
            yield return Hayden.Say("Arkalux help us...");
        }
    }

    private bool IsDeadPlayerCharacter(Character character)
    {
        return character && character.Faction == Battle.PlayerFaction && character.IsDead;
    }
}