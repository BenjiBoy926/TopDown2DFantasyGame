using System.Collections.Generic;
using UnityEngine;

public class GridSearchState
{
    public readonly Character Character;
    public readonly GridSearchStrategy Strategy;
    public readonly HashSet<Vector2Int> Visited;
    public readonly List<Node> SearchQueue;

    public GridSearchState(Character character, GridSearchStrategy strategy, HashSet<Vector2Int> visited, List<Node> searchQueue)
    {
        Character = character;
        Strategy = strategy;
        Visited = visited;
        SearchQueue = searchQueue;
    }
}