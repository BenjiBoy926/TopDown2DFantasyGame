using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class GridSearch : MonoBehaviour
{
    private Character _character;
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

    private Node FindFullPath(Vector2Int target) 
    {
        int CompareNodes(Node a, Node b)
        {
            int aCost = a.GetPathfindingCost(target);
            int bCost = b.GetPathfindingCost(target);
            return aCost.CompareTo(bCost);
        }

        Node targetNode = null;
        bool ExitCondition(Node node) => node.Cell == target;
        void ExitAction(Node node) => targetNode = node;
        Search(null, CompareNodes, ExitCondition, ExitAction);
        return targetNode;
    }

    public void FindAllCellsInRange(HashSet<Vector2Int> cellsInRange) 
    {
        cellsInRange.Clear();
        bool IsInRange(Node node) => node.StepsFromStart <= _character.TraversalRange;
        Search(IsInRange, null, null, null);
        foreach (var cell in _visited)
        {
            cellsInRange.Add(cell);
        }
    }

    private void Search(Predicate<Node> additionalEnqueueCondition, Comparison<Node> dequeueComparer, Predicate<Node> exitCondition, Action<Node> exitAction)
    {
        _visited.Clear();
        _searchQueue.Clear();

        Node start = new() { Cell = _character.CurrentCell, Parent = null };
        Enqueue(start);

        while (_searchQueue.Count > 0)
        {
            Node current = Dequeue(dequeueComparer);
            _visited.Add(current.Cell);

            if (exitCondition?.Invoke(current) == true)
            {
                exitAction?.Invoke(current);
                return;
            }

            VisitNeighbors(current, additionalEnqueueCondition);
        }
    }

    private void VisitNeighbors(Node node, Predicate<Node> additionalEnqueueCondition)
    {
        NodeNeighbors neighbors = NodeNeighbors.Get(node);
        Visit(neighbors.Left, additionalEnqueueCondition);
        Visit(neighbors.Right, additionalEnqueueCondition);
        Visit(neighbors.Up, additionalEnqueueCondition);
        Visit(neighbors.Down, additionalEnqueueCondition);
    }

    private void Visit(Node node, Predicate<Node> additionalEnqueueCondition)
    {
        if (ShouldEnqueue(node, additionalEnqueueCondition))
        {
            Enqueue(node);
        }
    }

    private bool ShouldEnqueue(Node node, Predicate<Node> additionalEnqueueCondition)
    {
        bool passesAdditionalCondition = additionalEnqueueCondition == null || additionalEnqueueCondition(node);
        return !_visited.Contains(node.Cell) && _character.IsPassable(node.Cell) && passesAdditionalCondition;
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

    private Node Dequeue(Comparison<Node> comparison)
    {
        if (_searchQueue.Count == 0)
            return null;

        if (comparison != null)
        {
            _searchQueue.Sort(comparison);
        }
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