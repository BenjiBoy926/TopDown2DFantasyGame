using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleSetup : MonoBehaviour
{
    private Battle _battle;

    public void Begin()
    {
        RegisterAllCharacters();
        RegisterAllSquads();
        RecordInitialState();
        _battle.StartPlayerTurn();
    }

    private void Awake()
    {
        _battle = GetComponent<Battle>();
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