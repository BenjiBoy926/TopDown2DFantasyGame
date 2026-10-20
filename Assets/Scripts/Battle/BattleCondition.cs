using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleCondition : MonoBehaviour
{
    private Battle _battle;

    private void Awake()
    {
        _battle = GetComponent<Battle>();
    }

    public bool IsWinConditionMet()
    {
        return AreAllEnemiesDead() && AreAllPlayersAlive();
    }

    private bool AreAllPlayersAlive()
    {
        foreach (var character in _battle.AllCharacters)
        {
            if (character.IsDead && character.Faction == _battle.PlayerFaction)
            {
                return false;
            }
        }
        return true;
    }

    private bool AreAllEnemiesDead()
    {
        foreach (var character in _battle.AllCharacters)
        {
            if (!character.IsDead && character.Faction != _battle.PlayerFaction)
            {
                return false;
            }
        }
        return true;
    }
}