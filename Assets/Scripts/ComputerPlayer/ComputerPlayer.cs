using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battle))]
[RequireComponent(typeof(ComputerPlayerMove))]
[RequireComponent(typeof(ComputerPlayerPathfinder))]
public class ComputerPlayer : MonoBehaviour
{
    public IReadOnlyCollection<Character> AllCharacters => _battle.AllCharacters;

    [SerializeField] private float _initialStartDelay = 1f;
    [SerializeField, ReadOnly] private List<Character> _characters = new();
    private Battle _battle;
    private Player _player;
    private Faction _faction;
    private readonly List<Character> _attackableCharacterScratch = new();
    private ComputerPlayerMove _move;
    private ComputerPlayerPathfinder _pathfinder;

    private void Awake()
    {
        _battle = GetComponent<Battle>();
        _player = GetComponentInChildren<Player>();
        _move = GetComponent<ComputerPlayerMove>();
        _pathfinder = GetComponent<ComputerPlayerPathfinder>();
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
            yield return _move.Move(character);
        }

        // Moving the charaters did not trigger the end of the turn, so we need to start the next turn manually
        if (_battle.CurrentFactionTurn == _faction)
        {
            _battle.StartNextTurn();
        }
    }

    public IEnumerator MoveToCell(Character character, Vector2Int cell)
    {
        return _pathfinder.MoveToCell(character, cell);
    }

    private void GetMoveableCharacters(List<Character> result)
    {
        // For now we just get all characters, but eventually we will group them by "squadrons"
        _battle.GetCharactersInFaction(_battle.CurrentFactionTurn, result);
        result.RemoveAll(ShouldNotMove);
    }

    private bool ShouldNotMove(Character character)
    {
        return !ShouldMove(character);
    }

    private bool ShouldMove(Character character)
    {
        return character.Faction == _faction && !character.IsDead;
    }
}
