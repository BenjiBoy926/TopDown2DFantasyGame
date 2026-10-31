using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;

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
        using (new ProfilerMarker("GridSearch.Search").Auto())
        {
            return SearchUnprofiled(strategy);
        }
    }

    private GridSearchResult SearchUnprofiled(GridSearchStrategy strategy)
    {
        _visited.Clear();
        _searchQueue.Clear();
        _state = new(_character, strategy, _visited, _searchQueue);

        _strategy = strategy;
        _strategy.Start(_state);

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
        Profiler.BeginSample("GridSearch.VisitNeighbors");
        NodeNeighbors neighbors = NodeNeighbors.Get(node);
        for (int i = 0; i < NodeNeighbors.Count; i++)
        {
            Node neighbor = _strategy.GetNeighbor(_state, neighbors, i);
            Visit(neighbor);
        }
        Profiler.EndSample();
    }

    private void Visit(Node node)
    {
        Profiler.BeginSample("GridSearch.Visit");
        if (ShouldEnqueue(node))
        {
            Enqueue(node);
        }
        Profiler.EndSample();
    }

    private bool ShouldEnqueue(Node node)
    {
        return !_visited.Contains(node.Cell) && _character.CouldWalkThroughCell(node.Cell) && _strategy.PassesCustomEnqueueConditions(_state, node);
    }

    private void Enqueue(Node node)
    {
        Profiler.BeginSample("GridSearch.Enqueue");
        int existingIndex = FindNodeInNext(node.Cell);
        if (existingIndex >= 0)
        {
            UpdateExistingNode(node, existingIndex);
        }
        else
        {
            _searchQueue.Add(node);
        }
        Profiler.EndSample();
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

        Profiler.BeginSample("GridSearch.Dequeue");
        int index = IndexOfLowestCostNode();
        Node node = _searchQueue[index];
        _searchQueue.RemoveAt(index);
        Profiler.EndSample();

        return node;
    }

    private int IndexOfLowestCostNode()
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