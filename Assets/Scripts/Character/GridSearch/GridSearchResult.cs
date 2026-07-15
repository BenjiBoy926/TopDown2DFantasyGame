using System.Collections.Generic;
using UnityEngine;

public class GridSearchResult
{
    public readonly Node ExitNode;
    public readonly HashSet<Vector2Int> VisitedCells = new();

    public GridSearchResult(Node exitNode, HashSet<Vector2Int> visitedCells)
    {
        ExitNode = exitNode;

        VisitedCells.Clear();
        foreach (var cell in visitedCells)
        {
            VisitedCells.Add(cell);
        }
    }
}   