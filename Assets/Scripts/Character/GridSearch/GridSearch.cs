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
    private GridSearchState _state;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public GridSearchResult Search(GridSearchStrategy strategy)
    {
        _strategy = strategy;
        _visited.Clear();
        _searchQueue.Clear();
        _state = new(_character, _strategy, _visited, _searchQueue);

        Node start = new() { Cell = _character.CurrentCell, Parent = null };
        Enqueue(start);

        while (_searchQueue.Count > 0)
        {
            Node current = Dequeue();
            _visited.Add(current.Cell);

            if (strategy.IsExitNode(_state, current))
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
        return !_visited.Contains(node.Cell) && _character.IsPassable(node.Cell) && _strategy.PassesCustomEnqueueConditions(_state, node);
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
}