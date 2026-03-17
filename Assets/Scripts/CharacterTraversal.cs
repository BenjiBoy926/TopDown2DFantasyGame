using System.Collections.Generic;
using UnityEngine;

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

    private readonly HashSet<Vector3Int> _traversibleTiles = new();
    private static readonly Queue<Vector3Int> _searchQueue = new();

    public void RecalculateTraversibleTiles(Character character)
    {
        _traversibleTiles.Clear();
        _searchQueue.Clear();

        const int MaxIterations = 100;
        int iterations = 0;

        Add(character.HomeCell);
        while (_searchQueue.Count > 0)
        {
            Vector3Int nextCell = _searchQueue.Dequeue();
            VisitNeighbors(character, nextCell);

            iterations++;
            if (iterations > MaxIterations)
            {
                Debug.LogError("Max iterations reached!");
                return;
            }
        }
    }

    private void VisitNeighbors(Character character, Vector3Int cell)
    {
        Neighbors neighbors = Neighbors.Get(cell);
        Visit(character, neighbors.Left);
        Visit(character, neighbors.Right);
        Visit(character, neighbors.Up);
        Visit(character, neighbors.Down);
    }

    private void Visit(Character character, Vector3Int cell)
    {
        if (ShouldAddCell(character, cell))
        {
            Add(cell);
        }
    }

    private bool ShouldAddCell(Character character, Vector3Int cell)
    {
        return !_traversibleTiles.Contains(cell) && character.IsTraversible(cell);
    }

    private void Add(Vector3Int cell)
    {
        _traversibleTiles.Add(cell);
        _searchQueue.Enqueue(cell);
    }
}
