using DG.Tweening;
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

    public IEnumerator Move(Character character)
    {
        _character = character;
        return Move();
    }

    private IEnumerator Move()
    {
        _character.RefreshRange();
        GetAttackableCharacters(_attackableCharacters);
        if (_attackableCharacters.Count > 0)
        {
            yield return AttackSomeone(_attackableCharacters);
        }
        else
        {
            // TODO: move to a better position first
            yield return _character.Defend();
        }
    }

    private void GetAttackableCharacters(List<Character> attackable)
    {
        attackable.Clear();
        foreach (var other in _computerPlayer.AllCharacters)
        {
            if (IsAttackable(other))
            {
                attackable.Add(other);
            }
        }
    }

    private bool IsAttackable(Character target)
    {
        return _character != target &&
            _character.Faction != target.Faction &&
            _character.IsReachable(target.CurrentCell) &&
            !target.IsDead;
    }

    private IEnumerator AttackSomeone(List<Character> attackable)
    {
        Character target = GetBestTarget(attackable);
        Vector2Int adjacentCell = GetBestAdjacentTile(target);
        Vector2 adjacentPosition = _character.CellToWorld(adjacentCell);
        _character.SetIsRunning(true);
        yield return _character.transform.DOMove(adjacentPosition, 1).WaitForCompletion();
        yield return _character.Attack(target);
    }

    private Character GetBestTarget(List<Character> attackable)
    {
        Character bestTarget = null;
        float bestScore = float.MinValue;
        foreach (var target in attackable)
        {
            float score = GetAttackScore(target);
            if (score > bestScore)
            {
                bestScore = score;
                bestTarget = target;
            }
        }
        return bestTarget;
    }

    private float GetAttackScore(Character target)
    {
        return GetAttackHealthScore(target) + GetAttackTravelScore(target);
    }

    private float GetAttackTravelScore(Character target)
    {
        Vector2Int targetCell = target.CurrentCell;
        return GetTravelScore(targetCell);
    }

    private float GetAttackHealthScore(Character target)
    {
        int healthBeforeAttack = target.CurrentHealth;
        int healthAfterAttack = target.CalculateHealthAfterHitFrom(_character);
        int damageDealt = healthBeforeAttack - healthAfterAttack;
        return (float)damageDealt / healthBeforeAttack;
    }

    private Vector2Int GetBestAdjacentTile(Character target)
    {
        CellNeighbors neighbors = CellNeighbors.Get(target.CurrentCell);
        List<Vector2Int> adjacentCells = new() { neighbors.Left, neighbors.Right, neighbors.Up, neighbors.Down };
        return adjacentCells[0]; // todo pick best stayable cell
    }

    private float GetTravelScore(Vector2Int targetCell)
    {
        Vector2Int myCell = _character.CurrentCell;
        int rectDistance = CharacterRange.RectangularDistance(myCell, targetCell);
        return (float)rectDistance / _character.TraversalRange;
    }
}