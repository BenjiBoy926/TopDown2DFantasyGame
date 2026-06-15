using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battle))]
public class ComputerPlayer : MonoBehaviour
{
    [SerializeField] private float _initialStartDelay = 1f;
    private Battle _battle;
    private Player _player;
    private Faction _faction;
    private readonly List<Character> _characters = new();

    private void Awake()
    {
        _battle = GetComponent<Battle>();
        _player = GetComponentInChildren<Player>();
    }

    private void OnEnable()
    {
        BattleTurn.NextTurnStarted += OnNextTurnStarted;
    }

    private void OnDisable()
    {
        BattleTurn.NextTurnStarted -= OnNextTurnStarted;
    }

    private void OnNextTurnStarted(Faction faction)
    {
        if (faction != _player.Faction)
        {
            _faction = faction;
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        while (_battle.IsTurnChangeAnimationPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(_initialStartDelay);

        GetMoveableCharacters(_characters);
        for (int i = 0; i < _characters.Count; i++)
        {
            Character character = _characters[i];
            yield return Move(character);
        }

        // Moving the charaters did not trigger the end of the turn, so we need to start the next turn manually
        if (_battle.CurrentFactionTurn == _faction)
        {
            _battle.StartNextTurn();
        }
    }

    private void GetMoveableCharacters(List<Character> result)
    {
        // For now we just get all characters, but eventually we will group them by "squadrons"
        _battle.GetCharactersInFaction(_battle.CurrentFactionTurn, result);
    }

    private IEnumerator Move(Character character)
    {
        yield return character.Defend(); //lol
    }
}
