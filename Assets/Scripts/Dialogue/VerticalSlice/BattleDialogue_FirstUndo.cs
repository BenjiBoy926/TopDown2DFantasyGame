using System.Collections;
using static UniqueCharacters;

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
            Character playerWhoWasDead = GetPlayerWhoWasDead();
            Character movedPlayer = GetMovedPlayer();
            if (playerWhoWasDead)
            {
                yield return UndoDeathDialogue(playerWhoWasDead);
            }
            else if (DidUndoEnemyTurn())
            {
                yield return UndoEnemyTurnDialogue();
            }
            else if (movedPlayer)
            {
                yield return UndoMoveDialogue(movedPlayer);
            }
            // No one was revived, enemy turn didn't undo, and no one actually moved.
            // Basically you made one character wait right where they are and then did an undo.
            // The characters have no comment to make
            else
            {
                Cancel();
            }
        }

        private IEnumerator UndoDeathDialogue(Character playerWhoWasDead)
        {
            yield return UndoDeathIntro(playerWhoWasDead);
            if (CountLiveMainCharacters() > 1)
            {
                yield return UndoDeath2PlusComments(playerWhoWasDead);
            }
        }

        private static IEnumerator UndoDeathIntro(Character playerWhoWasDead)
        {
            yield return Dialogue.FirstMatchingMainCharacterSay(playerWhoWasDead,
                "GAH! What was that?!",
                "What on earth?!",
                "Woah, WAOH! WHAT?!",
                "...What was that?");

            yield return Dialogue.FirstMatchingMainCharacterSay(playerWhoWasDead,
                "Did I just... die?!",
                "I thought I had just taken a fatal blow...",
                "I thought for sure I was done for!",
                "I had just taken a fatal blow...");

            yield return Dialogue.FirstMatchingMainCharacterSay(playerWhoWasDead,
                "I seem fine now... and I'm right back where I started...",
                "But now I seem fine. And I'm back where I was before...",
                "But now I feel great! And everything looks oddly familiar...",
                "But now everything is back to the way it was before.");

            yield return Dialogue.FirstMatchingMainCharacterSay(playerWhoWasDead,
                "It's like time just... turned back!",
                "It's like time just... turned back...",
                "Is this what time travel feels like?!",
                "It's like time just... turned back...");
        }

        private IEnumerator UndoDeath2PlusComments(Character playerWhoWasDead)
        {
            // If these are the only two characters alive then only "I" saw you die,
            // otherwise "we" saw you die
            int aliveCharacters = CountLiveMainCharacters();
            string IOrWe = aliveCharacters > 2 ? "We" : "I";
            bool OtherAliveCharacter(Character c) => c.IsAlive && c != playerWhoWasDead;
            yield return Dialogue.Say(OtherAliveCharacter,
                new(Hayden, $"It's not just you. {IOrWe} saw it too"),
                new(Gregory, $"I know. {IOrWe} saw it too."),
                new(Alfred, $"It's not in your head! {IOrWe} saw it too!"),
                new(Robin, $"...{IOrWe} saw it too."));

            yield return Dialogue.Say(OtherAliveCharacter,
                new(Hayden, $"It's good to see that you're alright! You had me worried there."),
                new(Gregory, $"I'm glad you're alright in any case."),
                new(Alfred, $"I'm just thrilled that you're okay now, you looked like you were really out!"),
                new(Robin, "Good thing you're okay now."));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Gregory, "Is this some goblin trick?"),
                new(Hayden, "I wonder if this is some kind of trap?"),
                new(Alfred, "Is this some stunt that the goblins are trying to pull?!"));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Robin, "I don't think so. There was a blue flash... it looked like it came from the Ember."),
                new(Gregory, "No... There was a blue flash... did it come from the Ember?"),
                new(Hayden, "But there was a blue flash... did it come from the Ember?"));

            yield return playerWhoWasDead.Say("How strange...");
        }

        private IEnumerator UndoEnemyTurnDialogue()
        {
            yield return Dialogue.FirstLiveCharacterSay(
                new(Gregory, "That's strange. It looks like the goblins retreated..."),
                new(Hayden, "Huh? It looks like the goblins just retreated..."),
                new(Alfred, "What's going on? They're running for the hills!"),
                new(Robin, "That makes no sense. It looks like they retreated..."));

            if (CountLiveMainCharacters() > 1)
            {
                yield return UndoEnemyTurn2PlusComments();
            }
        }

        private static IEnumerator UndoEnemyTurn2PlusComments()
        {
            yield return Dialogue.FirstLiveCharacterSay(
                new(Hayden, "There was a bright blue flash and suddenly they all moved back!"),
                new(Gregory, "I saw a blue flash and then... they all moved back."),
                new(Alfred, "There was a bright blue flash and suddenly they all moved back!"));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Gregory, "Is this some goblin trick?"),
                new(Hayden, "I wonder if this is some kind of trap?"),
                new(Alfred, "Is this some stunt that the goblins are trying to pull?!"));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Robin, "I don't think so. The blue flash looked like it came from the Ember."),
                new(Gregory, "No... The blue flash looked like it came from the Ember."),
                new(Hayden, "But the blue flash looked like it came from the Ember..."));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Gregory, "How strange..."),
                new(Alfred, "How bizarre!"),
                new(Robin, "How strange..."));
        }

        private IEnumerator UndoMoveDialogue(Character movedPlayer)
        {
            yield return UndoMoveIntro(movedPlayer);
            if (CountLiveMainCharacters() > 1)
            {
                yield return UndoMove2PlusComments(movedPlayer);
            }
        }

        private static IEnumerator UndoMoveIntro(Character movedPlayer)
        {
            yield return Dialogue.FirstMatchingMainCharacterSay(movedPlayer,
                "What was that?",
                "What on earth?",
                "Woah, woah! What?!",
                "...What was that?");

            yield return Dialogue.FirstMatchingMainCharacterSay(movedPlayer,
                "I was just over there...",
                "I was just over there...",
                "Wasn't I just over there?!",
                "I was just over there...");

            yield return Dialogue.FirstMatchingMainCharacterSay(movedPlayer,
                "But now I'm back where I started!",
                "But now I'm back where I started...",
                "But now I'm back where I started!",
                "But now I'm back where I started...");

            yield return Dialogue.FirstMatchingMainCharacterSay(movedPlayer,
                "It's like time just... turned back!",
                "It's like time turned back...",
                "Is this what time travel feels like?!",
                "It's like time turned back...");
        }

        private IEnumerator UndoMove2PlusComments(Character movedPlayer)
        {
            // If these are the only two characters alive then only "I" saw you die,
            // otherwise "we" saw you die
            string IOrWe = CountLiveMainCharacters() > 2 ? "We" : "I";
            bool OtherCharacter(Character c) => c.IsAlive && c != movedPlayer;
            yield return Dialogue.Say(OtherCharacter,
                new(Hayden, $"It's not just you. {IOrWe} saw it too"),
                new(Gregory, $"I know. {IOrWe} saw it too."),
                new(Alfred, $"It's not in your head! {IOrWe} saw it too!"),
                new(Robin, $"...{IOrWe} saw it too."));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Gregory, "Is this some goblin trick?"),
                new(Hayden, "I wonder if this is some kind of trap?"),
                new(Alfred, "Is this some stunt that the goblins are trying to pull?!"));

            yield return Dialogue.FirstLiveCharacterSay(
                new(Robin, "I don't think so. There was a blue flash... it looked like it came from the Ember."),
                new(Gregory, "No... There was a blue flash... did it come from the Ember."),
                new(Hayden, "But there was a blue flash... did it come from the Ember?"));

            yield return movedPlayer.Say("How strange...");
        }

        private Character GetPlayerWhoWasDead()
        {
            for (int i = Battle.CurrentStateIndex + 1; i < Battle.StateCount; i++)
            {
                BattleState state = Battle.GetState(i);
                Character character = GetPlayerWhoWasDead(state);
                if (character)
                {
                    return character;
                }
            }
            return null;
        }

        private Character GetPlayerWhoWasDead(BattleState state)
        {
            CharacterRecord playerCharacterRecord = GetPlayerRecord(state);
            if (playerCharacterRecord.Character && playerCharacterRecord.Character.IsAlive && playerCharacterRecord.State.Health <= 0)
            {
                return playerCharacterRecord.Character;
            }
            return null;
        }

        private bool DidUndoEnemyTurn()
        {
            return Battle.LatestStateIndex - Battle.CurrentStateIndex > 1;
        }

        private Character GetMovedPlayer()
        {
            BattleState undoneState = Battle.GetState(Battle.CurrentStateIndex + 1);
            CharacterRecord record = GetPlayerRecord(undoneState);
            bool didCellChange = record.Character.CurrentCell != record.State.Cell;
            bool didHealthChange = record.Character.CurrentHealth != record.State.Health;
            bool didAnythingChange = didCellChange || didHealthChange;
            return didAnythingChange ? record.Character : null;
        }

        private CharacterRecord GetPlayerRecord(BattleState state)
        {
            for (int i = 0; i < state.RecordCount; i++)
            {
                CharacterRecord record = state.GetRecord(i);
                if (record.Character.Faction == Battle.PlayerFaction)
                {
                    return record;
                }
            }
            return default;
        }

        private int CountLiveMainCharacters()
        {
            int count = 0;
            for (int i = 0; i < MainCharacters.Count; i++)
            {
                if (MainCharacters[i].IsAlive)
                {
                    count++;
                }
            }
            return count;
        }
    }
}