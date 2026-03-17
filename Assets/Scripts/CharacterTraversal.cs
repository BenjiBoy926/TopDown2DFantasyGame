using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterTraversal : MonoBehaviour
{
    private struct Neighbors
    {
        public Vector3Int Left, Right, Up, Down;

        public static Neighbors Get(Vector3Int center)
        {
            return new Neighbors
            {
                Left = center + Vector3Int.left,
                Right = center + Vector3Int.right,
                Up = center + Vector3Int.up,
                Down = center + Vector3Int.down
            };
        }
    }

    public IReadOnlyCollection<Vector3Int> TraversibleTiles => _traversibleTiles;

    private Character _character;
    private readonly HashSet<Vector3Int> _traversibleTiles = new();
    private static readonly Queue<Vector3Int> _searchQueue = new();

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void RecalculateTraversibleTiles()
    {
        _traversibleTiles.Clear();
        _searchQueue.Clear();

        const int MaxIterations = 100;
        int iterations = 0;

        Add(_character.HomeCell);
        while (_searchQueue.Count > 0)
        {
            Vector3Int nextCell = _searchQueue.Dequeue();
            VisitNeighbors(nextCell);

            iterations++;
            if (iterations > MaxIterations)
            {
                Debug.LogError("Max iterations reached!");
                return;
            }
        }
    }

    public Vector2 ClampToTraversibleTiles(Vector2 position)
    {
        Vector3Int currentCell = _character.WorldToCell(position);
        if (_traversibleTiles.Contains(currentCell)) return position;

        Vector3Int closestTraversibleCell = ClosestTraversibleCell(currentCell);
        return _character.CellToWorld(closestTraversibleCell);
    }

    private void VisitNeighbors(Vector3Int cell)
    {
        Neighbors neighbors = Neighbors.Get(cell);
        Visit(neighbors.Left);
        Visit(neighbors.Right);
        Visit(neighbors.Up);
        Visit(neighbors.Down);
    }

    private void Visit(Vector3Int cell)
    {
        if (ShouldAddCell(cell))
        {
            Add(cell);
        }
    }

    private bool ShouldAddCell(Vector3Int cell)
    {
        return !_traversibleTiles.Contains(cell) && _character.IsTraversible(cell);
    }

    private void Add(Vector3Int cell)
    {
        _traversibleTiles.Add(cell);
        _searchQueue.Enqueue(cell);
    }

    private Vector3Int ClosestTraversibleCell(Vector3Int input)
    {
        if (_traversibleTiles.Count == 0) return input;

        Vector3Int closestCell = input;
        float closestDistance = float.MaxValue;
        foreach (Vector3Int cell in _traversibleTiles)
        {
            Vector3Int offset = cell - input;
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
