using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(ComputerPlayer))]
public class ComputerPlayerMove : MonoBehaviour
{
    [SerializeField, ReadOnly] private Character _character;
    [SerializeField, ReadOnly] private List<Character> _attackableCharacters = new();
    private ComputerPlayer _computerPlayer;

    private void Awake()
    {
        _computerPlayer = GetComponent<ComputerPlayer>();
    }

    public Coroutine Move(Character character)
    {
        _character = character;
        return Move();
    }

    private Coroutine Move()
    {
        _character.RefreshRange();
        GetAttackableCharacters(_character, _attackableCharacters);
        if (_attackableCharacters.Count > 0)
        {
            return AttackSomeone(_character, _attackableCharacters);
        }
        else
        {
            // TODO: move to a better position first
            return _character.Defend();
        }
    }

    private void GetAttackableCharacters(Character character, List<Character> attackable)
    {
        attackable.Clear();
        foreach (var other in _computerPlayer.AllCharacters)
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
            float score = GetAttackScore(character, target);
            if (score > bestScore)
            {
                bestScore = score;
                bestTarget = target;
            }
        }
        return bestTarget;
    }

    private float GetAttackScore(Character character, Character target)
    {
        return GetAttackHealthScore(character, target) + GetAttackTravelScore(character, target);
    }

    private static float GetAttackTravelScore(Character character, Character target)
    {
        Vector2Int targetCell = target.CurrentCell;
        return GetTravelScore(character, targetCell);
    }

    private static float GetAttackHealthScore(Character character, Character target)
    {
        int healthBeforeAttack = target.CurrentHealth;
        int healthAfterAttack = target.CalculateHealthAfterHitFrom(character);
        int damageDealt = healthBeforeAttack - healthAfterAttack;
        return (float)damageDealt / healthBeforeAttack;
    }

    private Vector2Int GetBestAdjacentTile(Character character, Character target)
    {
        CellNeighbors neighbors = CellNeighbors.Get(target.CurrentCell);
        List<Vector2Int> adjacentCells = new() { neighbors.Left, neighbors.Right, neighbors.Up, neighbors.Down };
        return adjacentCells[0]; // todo pick best stayable cell
    }

    private static float GetTravelScore(Character character, Vector2Int targetCell)
    {
        Vector2Int myCell = character.CurrentCell;
        int rectDistance = CharacterRange.RectangularDistance(myCell, targetCell);
        return (float)rectDistance / character.TraversalRange;
    }
}