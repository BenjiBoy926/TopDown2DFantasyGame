using System.Collections;
using UnityEngine;
using static UniqueCharacters;

namespace VerticalSlice
{
    public class BattleDialogue_GrugIntro : BattleDialogueSequence
    {
        public override TriggerType Trigger => TriggerType.TurnStart;

        public override bool ShouldPlay(Character interactor, Character target)
        {
            Squad grugSquad = Battle.GetSquad(Grug);
            bool isSquadAwake = grugSquad && (grugSquad.ShouldWakeUp() || grugSquad.IsAwake);
            bool areEnoughPlayersAlive = (Robin.IsAlive || Hayden.IsAlive) && (Gregory.IsAlive || Alfred.IsAlive);
            return !HasPlayed && Battle.CurrentFactionTurn != Battle.PlayerFaction && isSquadAwake && !Grug.IsDead && areEnoughPlayersAlive;
        }

        protected override IEnumerator GetSequence(Character interactor, Character target)
        {
            yield return Battle.CameraGlide(Grug.transform);
            yield return Grug.Say("Tsk. They're a tenacious lot, I'll give them that.");
            yield return Grug.Say("No matter. They'll be no match for me.");
            yield return Grug.Say("Onward, worms! Show yourselves worthy of Ha'all!");

            yield return Battle.CameraGlide(GetAveragePlayerPosition());

            yield return Dialogue.FirstLiveCharacterSay(
                new(Robin, "Uh-oh"),
                new(Hayden, "...No! It can't be. Is that...?"));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Hayden, "What? What is it?"),
                new(Gregory, "What is it?"),
                new(Alfred, "Huh? What's the matter?"));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Robin, "Their leader. The brute in the center. He's not a goblin. He's an orc."),
                new(Hayden, "It's hard to see in this light but... I don't think their leader is a goblin. I think he's an orc."));

            if (Hayden.IsAlive)
            {
                yield return Hayden.Say("WHAT?! How can that be?!");
            }

            yield return Dialogue.FirstLiveCharacterSay(
                new(Gregory, "Sorry... what's an orc?"),
                new(Alfred, "Boy are we in for it! ...Wait, what's an orc?"));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Hayden, "Orcs come from an ancient race of humans that Arkalux cursed for their wickedness in the distant past"),
                new(Robin, "They descend from an ancient race of cursed humans. Or say the legends say."));

            if (Hayden.IsAlive)
            {
                yield return Hayden.Say("But no one has seen an orc for centuries! To see one here, now, what could it mean...?!");
            }

            yield return Dialogue.FirstLiveCharacterSay(
                new(Alfred, "Enough ancient history. How do we kill it?"),
                new(Gregory, "Strange. How does this affect our strategy?"));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Robin, "It'll take more hits, but eventually it will go down like any other goblin."),
                new(Hayden, "Oh? Uhm... it's not immortal or anything, but it might take more hits than the average goblin."));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Gregory, "Brace yourselves. Here they come!"),
                new(Alfred, "Here they come, let's give them hell!"));
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
}