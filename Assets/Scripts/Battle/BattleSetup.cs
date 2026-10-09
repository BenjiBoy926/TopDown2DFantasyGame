using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleSetup : MonoBehaviour
{
    private Battle _battle;

    public void PerformSetupRegistration(Battle battle)
    {
        _battle = battle;
        RegisterAllCharacters();
        RegisterAllSquads();
    }

    public void Begin()
    {
        _battle.StartPlayerTurn();
        RecordInitialState();
    }

    private void RegisterAllCharacters()
    {
        Character[] characters = GetComponentsInChildren<Character>();
        foreach (var character in characters)
        {
            _battle.Register(character);
        }
    }

    private void RegisterAllSquads()
    {
        Squad[] squads = GetComponentsInChildren<Squad>();
        foreach (var squad in squads)
        {
            _battle.Register(squad);
        }
    }

    private void RecordInitialState()
    {
        _battle.RecordInitialState();
    }
}