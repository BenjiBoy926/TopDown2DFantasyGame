using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Character))]
public class CharacterRange : MonoBehaviour
{
    private struct Neighbors
    {
        public Vector2Int Left, Right, Up, Down;

        public static Neighbors Get(Vector2Int center)
        {
            return new Neighbors
            {
                Left = center + Vector2Int.left,
                Right = center + Vector2Int.right,
                Up = center + Vector2Int.up,
                Down = center + Vector2Int.down
            };
        }
    }

    public IReadOnlyCollection<Vector2Int> TraversibleCells => _traversibleCells;
    public IReadOnlyCollection<Vector2Int> AttackableEdgeCells => _attackableEdgeCells;

    private Character _character;
    private readonly HashSet<Vector2Int> _traversibleCells = new();
    private readonly HashSet<Vector2Int> _attackableEdgeCells = new();
    private readonly HashSet<Vector2Int> _reachableCells = new();

    private static readonly Queue<Vector2Int> _searchQueue = new();

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void Refresh()
    {
        RecalculateTraversibleCells();
        RecalculateAttackableEdgeCells();

        _reachableCells.Clear();
        _reachableCells.UnionWith(_traversibleCells);
        _reachableCells.UnionWith(_attackableEdgeCells);
    }

    private void RecalculateTraversibleCells()
    {
        _traversibleCells.Clear();
        _searchQueue.Clear();

        const int MaxIterations = 100;
        int iterations = 0;

        Add(_character.HomeCell);
        while (_searchQueue.Count > 0)
        {
            Vector2Int nextCell = _searchQueue.Dequeue();
            VisitNeighbors(nextCell);

            iterations++;
            if (iterations > MaxIterations)
            {
                Debug.LogError("Max iterations reached!");
                break;
            }
        }
    }

    private void RecalculateAttackableEdgeCells()
    {
        _attackableEdgeCells.Clear();
        foreach (var traversibleCell in _traversibleCells)
        {
            Neighbors neighbors = Neighbors.Get(traversibleCell);
            CheckAttackableEdgeCell(neighbors.Left);
            CheckAttackableEdgeCell(neighbors.Right);
            CheckAttackableEdgeCell(neighbors.Up);
            CheckAttackableEdgeCell(neighbors.Down);
        }
    }

    public Vector2 ClampToTraversibleCells(Vector2 position)
    {
        return ClampToCells(position, _traversibleCells);
    }

    public Vector2 ClampToReachableCells(Vector2 position)
    {
        return ClampToCells(position, _reachableCells);
    }

    private void VisitNeighbors(Vector2Int cell)
    {
        Neighbors neighbors = Neighbors.Get(cell);
        Visit(neighbors.Left);
        Visit(neighbors.Right);
        Visit(neighbors.Up);
        Visit(neighbors.Down);
    }

    private void Visit(Vector2Int cell)
    {
        if (ShouldAddCell(cell))
        {
            Add(cell);
        }
    }

    private bool ShouldAddCell(Vector2Int cell)
    {
        return !_traversibleCells.Contains(cell) && _character.IsTraversible(cell);
    }

    private void Add(Vector2Int cell)
    {
        _traversibleCells.Add(cell);
        _searchQueue.Enqueue(cell);
    }

    public Vector2Int ClosestTraversibleCell(Vector2 input)
    {
        return ClosestCell(input, _traversibleCells);
    }

    private void CheckAttackableEdgeCell(Vector2Int cell)
    {
        if (!_traversibleCells.Contains(cell))
        {
            _attackableEdgeCells.Add(cell);
        }
    }

    private Vector2 ClampToCells(Vector2 position, HashSet<Vector2Int> cells)
    {
        Vector2Int currentCell = _character.WorldToCell(position);
        if (cells.Contains(currentCell)) return position;

        Vector2Int closestTraversibleCell = ClosestCell(position, cells);
        Vector2 cellPosition = _character.CellToWorld(closestTraversibleCell);

        float xExtent = _character.CellWidth / 2;
        float yExtent = _character.CellHeight / 2;
        Rect range = Rect.MinMaxRect(cellPosition.x - xExtent, cellPosition.y - yExtent, cellPosition.x + xExtent, cellPosition.y + yExtent);

        float x = Mathf.Clamp(position.x, range.xMin, range.xMax);
        float y = Mathf.Clamp(position.y, range.yMin, range.yMax);

        return new(x, y);
    }

    private Vector2Int ClosestCell(Vector2 input, HashSet<Vector2Int> cells)
    {
        Vector2Int inputCell = _character.WorldToCell(input);
        if (cells.Count == 0) return inputCell;

        Vector2Int closestCell = Vector2Int.zero;
        float closestDistance = float.MaxValue;
        foreach (Vector2Int cell in cells)
        {
            Vector2 cellPosition = _character.CellToWorld(cell);
            Vector2 offset = input - cellPosition;
            float currentDistance = offset.sqrMagnitude;
            if (currentDistance < closestDistance)
            {
                closestCell = cell;
                closestDistance = currentDistance;
            }
        }
        return closestCell;
    }
}
