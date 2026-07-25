using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battle))]
[RequireComponent(typeof(ComputerPlayerMove))]
public class ComputerPlayer : MonoBehaviour
{
    public IReadOnlyCollection<Character> AllCharacters => _battle.AllCharacters;

    [SerializeField] private float _initialStartDelay = 1f;
    [SerializeField, ReadOnly] private List<Character> _characters = new();
    private Battle _battle;
    private Player _player;
    private Faction _faction;
    private ComputerPlayerMove _move;

    private void Awake()
    {
        _battle = GetComponent<Battle>();
        _player = GetComponentInChildren<Player>();
        _move = GetComponent<ComputerPlayerMove>();
    }

    private void OnEnable()
    {
        BattleTurn.NextTurnStarted += OnNextTurnStarted;
    }

    private void OnDisable()
    {
        BattleTurn.NextTurnStarted -= OnNextTurnStarted;
    }

    public void CameraFollow(Transform target)
    {
        _battle.CameraFollow(target);
    }

    public void CameraUnfollow()
    {
        _battle.CameraUnfollow();
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

    private void GetMoveableCharacters(List<Character> result)
    {
        result.Clear();
        foreach (var squad in _battle.AllSquads)
        {
            squad.Refresh();
        }
        foreach (var squad in _battle.AllSquads)
        {
            if (squad.IsAwake)
            {
                AddCharactersInSquad(squad, result);
            }
        }
    }

    private void AddCharactersInSquad(Squad squad, List<Character> result)
    {
        foreach (var member in squad.Members)
        {
            if (ShouldMove(member))
            {
                result.Add(member);
            }
        }
    }

    private bool ShouldMove(Character character)
    {
        return character.Faction == _faction && !character.IsDead;
    }
}
