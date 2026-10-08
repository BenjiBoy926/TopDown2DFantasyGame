using UnityEngine;
using UnityEngine.InputSystem;

public class DebugCommands : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Break();
        }
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            InstantVictory();
        }
    }

    private void InstantVictory()
    {
        Battle battle = FindAnyObjectByType<Battle>();
        if (!battle.IsInProgress)
            return;

        foreach (var character in battle.AllCharacters)
        {
            if (character.Faction != battle.PlayerFaction)
            {
                character.SetHealth(0);
            }
        }
        // Hayden killed them all, lol
        battle.NotifyCharacterMoveFinished(UniqueCharacters.Hayden);
    }
}