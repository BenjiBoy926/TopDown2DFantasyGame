using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
            return AttackBestTarget(_attackableCharacters);
        }
        else
        {
            return DefendBestCell();
        }
    }

    private void GetAttackableCharacters(List<Character> attackable)
    {
        attackable.Clear();
        IEnumerable<Character> attackableIterator = _computerPlayer.AllCharacters.Where(IsAttackable);
        attackable.AddRange(attackableIterator);
    }

    private bool IsAttackable(Character target)
    {
        return IsTargettable(target) && IsReachable(target);
    }

    private bool IsReachable(Character target)
    {
        return _character.IsReachable(target.CurrentCell);
    }

    private IEnumerator AttackBestTarget(List<Character> attackable)
    {
        Character target = GetBestTarget(attackable);
        Vector2Int adjacentCell = GetBestAdjacentTile(target);
        yield return MoveToCell(adjacentCell);
        yield return _character.Attack(target);
    }

    private IEnumerator MoveToCell(Vector2Int adjacentCell)
    {
        return _computerPlayer.MoveToCell(_character, adjacentCell);
    }

    private Character GetBestTarget(List<Character> attackable)
    {
        attackable.Sort(CompareTargets);
        return attackable[^1];
    }

    private int CompareTargets(Character a, Character b)
    {
        return GetAttackScore(a).CompareTo(GetAttackScore(b));
    }

    private float GetAttackScore(Character target)
    {
        return GetAttackDamageScore(target);
    }

    private float GetAttackDamageScore(Character target)
    {
        int healthBeforeAttack = target.CurrentHealth;
        int healthAfterAttack = target.CalculateHealthAfterHitFrom(_character);
        int damageDealt = healthBeforeAttack - healthAfterAttack;
        return (float)damageDealt / healthBeforeAttack;
    }

    private IEnumerator DefendBestCell()
    {
        Character closestEnemy = GetClosestEnemy();
        Vector2Int adjacentCell = GetBestAdjacentTile(closestEnemy);
        yield return _computerPlayer.MoveToCell(_character, adjacentCell);
        yield return _character.Defend();
    }

    private Character GetClosestEnemy()
    {
        return _computerPlayer.AllCharacters.Where(IsTargettable).OrderBy(RectangularDistanceToTarget).FirstOrDefault();
    }

    private bool IsTargettable(Character target)
    {
        return _character != target && _character.Faction != target.Faction && !target.IsDead;
    }

    private Vector2Int GetBestAdjacentTile(Character target)
    {
        CellNeighbors neighbors = CellNeighbors.Get(target.CurrentCell);
        List<Vector2Int> adjacentCells = new() { neighbors.Left, neighbors.Right, neighbors.Up, neighbors.Down };
        return GetBestStayableCell(adjacentCells);
    }

    private bool IsStayable(Vector2Int cell)
    {
        return _character.IsStayable(cell);
    }

    private Vector2Int GetBestStayableCell(IReadOnlyCollection<Vector2Int> cells)
    {
        return cells.Where(IsStayable).OrderByDescending(GetCellScore).FirstOrDefault();
    }

    private float GetCellScore(Vector2Int targetCell)
    {
        bool CanStayInCell(Character other) => other.IsStayable(targetCell);
        int alliesThatCanStayInThisCell = _computerPlayer.AllCharacters.Where(IsAlly).Count(CanStayInCell);
        return -alliesThatCanStayInThisCell;
    }

    private bool IsAlly(Character other)
    {
        return other != _character && other.Faction == _character.Faction;
    }

    private int RectangularDistanceToTarget(Character target)
    {
        return CharacterRange.RectangularDistance(_character.CurrentCell, target.CurrentCell);
    }
}