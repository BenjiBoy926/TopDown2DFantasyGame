using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class GridSearch : MonoBehaviour
{
    private Character _character;
    private GridSearchStrategy _strategy;
    private readonly HashSet<Vector2Int> _visited = new();
    private readonly List<Node> _searchQueue = new();

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void FindPath(Vector2Int target, List<Vector2Int> path)
    {
        Node targetNode = FindFullPath(target);
        if (targetNode != null)
        {
            ReconstructLimitedPath(targetNode, path);
        }
    }

    public void FindPathToNearestEnemy(List<Vector2Int> path)
    {
        Node targetNode = FindFullPathToNearestEnemy();
        if (targetNode != null)
        {
            ReconstructLimitedPath(targetNode, path);
        }
    }

    private Node FindFullPath(Vector2Int target) 
    {
        var strategy = new GridSearchStrategy.FindPathToCell(target);
        var result = Search(strategy);
        return result.ExitNode;
    }

    private Node FindFullPathToNearestEnemy()
    {
        var strategy = new GridSearchStrategy.FindPathToNearestEnemy(_character);
        var result = Search(strategy);
        return result.ExitNode;
    }

    public void FindAllCellsInRange(HashSet<Vector2Int> cellsInRange) 
    {
        var strategy = new GridSearchStrategy.FindAllCellsInRange(_character);
        var result = Search(strategy);
        foreach (var cell in result.VisitedCells)
        {
            cellsInRange.Add(cell);
        }
    }

    private GridSearchResult Search(GridSearchStrategy strategy)
    {
        _strategy = strategy;
        _visited.Clear();
        _searchQueue.Clear();

        Node start = new() { Cell = _character.CurrentCell, Parent = null };
        Enqueue(start);

        while (_searchQueue.Count > 0)
        {
            Node current = Dequeue();
            _visited.Add(current.Cell);

            if (strategy.IsExitNode(current))
            {
                return new(current, _visited);
            }

            VisitNeighbors(current);
        }

        return new(null, _visited);
    }

    private void VisitNeighbors(Node node)
    {
        NodeNeighbors neighbors = NodeNeighbors.Get(node);
        foreach (var neighbor in neighbors)
        {
            Visit(neighbor);
        }
    }

    private void Visit(Node node)
    {
        if (ShouldEnqueue(node))
        {
            Enqueue(node);
        }
    }

    private bool ShouldEnqueue(Node node)
    {
        return !_visited.Contains(node.Cell) && _character.IsPassable(node.Cell) && _strategy.PassesCustomEnqueueConditions(node);
    }

    private void Enqueue(Node node)
    {
        int existingIndex = FindNodeInNext(node.Cell);
        if (existingIndex >= 0)
        {
            UpdateExistingNode(node, existingIndex);
        }
        else
        {
            _searchQueue.Add(node);
        }
    }

    private void UpdateExistingNode(Node node, int existingIndex)
    {
        if (node.StepsFromStart < _searchQueue[existingIndex].StepsFromStart)
        {
            _searchQueue[existingIndex] = node;
        }
    }

    private Node Dequeue()
    {
        if (_searchQueue.Count == 0)
            return null;

        _searchQueue.Sort(_strategy.NodeComparison);
        Node node = _searchQueue[0];
        _searchQueue.RemoveAt(0);
        return node;
    }

    private int FindNodeInNext(Vector2Int cell)
    {
        return _searchQueue.FindIndex(n => n.Cell == cell);
    }

    private void ReconstructLimitedPath(Node node, List<Vector2Int> path)
    {
        path.Clear();
        while (node != null)
        {
            if (node.StepsFromStart <= _character.TraversalRange)
            {
                path.Add(node.Cell);
            }
            node = node.Parent;
        }
        path.Reverse();
    }
}