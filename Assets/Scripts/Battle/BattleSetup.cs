using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleSetup : MonoBehaviour
{
    private Battle _battle;

    public void Begin()
    {
        RegisterAllCharacters();
        RecordInitialState();
        _battle.StartFirstTurn();
    }

    private void Awake()
    {
        _battle = GetComponent<Battle>();
    }

    private void RegisterAllCharacters()
    {
        Character[] characters = GetComponentsInChildren<Character>(true);
        foreach (var character in characters)
        {
            _battle.Register(character);
            character.SetBattle(_battle);
        }
    }

    private void RecordInitialState()
    {
        _battle.RecordInitialState();
    }
}