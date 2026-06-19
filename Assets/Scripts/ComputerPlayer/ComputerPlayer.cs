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
    private readonly List<Character> _attackableCharacterScratch = new();

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

    private IEnumerator Move(Character character)
    {
        character.RefreshRange();
        GetAttackableCharacters(character, _attackableCharacterScratch);
        if (_attackableCharacterScratch.Count > 0)
        {
            yield return AttackSomeone(character, _attackableCharacterScratch);
        }
        else
        {
            // TODO: move to a better position first
            yield return character.Defend();
        }
    }

    private void GetAttackableCharacters(Character character, List<Character> attackable)
    {
        attackable.Clear();
        foreach (var other in _battle.AllCharacters)
        {
            if (IsAttackable(character, other))
            {
                attackable.Add(other);
            }
        }
    }

    private bool IsAttackable(Character character, Character target)
    {
        return character != target &&
            character.Faction != target.Faction &&
            character.IsReachable(target.CurrentCell) &&
            !target.IsDead;
    }

    private Coroutine AttackSomeone(Character character, List<Character> attackable)
    {
        Character target = GetBestTarget(character, attackable);
        return character.Attack(target);
    }

    private Character GetBestTarget(Character character, List<Character> attackable)
    {
        Character bestTarget = null;
        float bestScore = float.MinValue;
        foreach (var target in attackable)
        {
            float score = ScoreAttack(character, target);
            if (score > bestScore)
            {
                bestScore = score;
                bestTarget = target;
            }
        }
        return bestTarget;
    }

    private float ScoreAttack(Character character, Character target)
    {
        return 0;
    }
}
