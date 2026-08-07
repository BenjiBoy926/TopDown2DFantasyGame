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
        for (int i = 0; i < NodeNeighbors.Count; i++)
        {
            Visit(neighbors[i]);
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
        return !_visited.Contains(node.Cell) && _character.CouldWalkThroughCell(node.Cell) && _strategy.PassesCustomEnqueueConditions(_state, node);
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

        int index = FindIndexOfLowestCostNode();
        Node node = _searchQueue[index];
        _searchQueue.RemoveAt(index);
        return node;
    }

    private int FindIndexOfLowestCostNode()
    {
        if (_searchQueue.Count == 0)
            return -1;

        int bestIndex = 0;
        int bestCost = _strategy.GetNodeCost(_state, _searchQueue[bestIndex]);
        for (int i = 1; i < _searchQueue.Count; i++)
        {
            int currentCost = _strategy.GetNodeCost(_state, _searchQueue[i]);
            if (currentCost < bestCost)
            {
                bestIndex = i;
                bestCost = currentCost;
            }
        }
        return bestIndex;
    }

    private int FindNodeInNext(Vector2Int cell)
    {
        // Can't use "FindIndex" because you have to allocate a delegate
        for (int i = 0; i < _searchQueue.Count; i++)
        {
            Node node = _searchQueue[i];
            if (node.Cell == cell)
            {
                return i;
            }
        }
        return -1;
    }
}