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