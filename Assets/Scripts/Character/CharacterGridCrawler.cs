using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Character))]
public class CharacterGridCrawler : MonoBehaviour
{
    public class Node : IEquatable<Node>
    {
        public int StepsFromStart => Parent != null ? Parent.StepsFromStart + 1 : 0;

        public Vector2Int Cell;
        public Node Parent;

        public int GetPathfindingCost(Vector2Int target)
        {
            return StepsFromStart + GetEstimatedStepsToEnd(target);
        }

        public int GetEstimatedStepsToEnd(Vector2Int target)
        {
            return CharacterRange.RectangularDistance(Cell, target);
        }

        public bool Equals(Node other)
        {
            return other != null && Cell == other.Cell;
        }

        public override int GetHashCode()
        {
            return Cell.GetHashCode();
        }
    }

    public class NodeNeighbors
    {
        public Node Left, Right, Up, Down;

        public static NodeNeighbors Get(Node node)
        {
            CellNeighbors neighbors = CellNeighbors.Get(node.Cell);
            return new NodeNeighbors
            {
                Left = new Node { Cell = neighbors.Left, Parent = node },
                Right = new Node { Cell = neighbors.Right, Parent = node },
                Up = new Node { Cell = neighbors.Up, Parent = node },
                Down = new Node { Cell = neighbors.Down, Parent = node }
            };
        }
    }

    public IReadOnlyCollection<Node> Visited => _visited;

    private Character _character;
    private readonly HashSet<Node> _visited = new();
    private readonly List<Node> _next = new();
    private readonly List<Vector2Int> _path = new();

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public List<Vector2Int> FindPath(Vector2Int target)
    {
        Node targetNode = FindFullPath(target);
        if (targetNode != null)
        {
            return ReconstructLimitedPath(targetNode);
        }
        else
        {
            return new();
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

    public void FindAllCellsInRange() 
    {
        bool IsInRange(Node node) => node.StepsFromStart <= _character.TraversalRange;
        Search(IsInRange, null, null, null);
    }

    private void Search(Predicate<Node> additionalEnqueueCondition, Comparison<Node> dequeueComparer, Predicate<Node> exitCondition, Action<Node> exitAction)
    {
        _visited.Clear();
        _next.Clear();

        Node start = new() { Cell = _character.CurrentCell, Parent = null };
        Enqueue(start);

        while (_next.Count > 0)
        {
            Node current = Dequeue(dequeueComparer);
            _visited.Add(current);

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
        return !_visited.Contains(node) && _character.IsPassable(node.Cell) && passesAdditionalCondition;
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
            _next.Add(node);
        }
    }

    private void UpdateExistingNode(Node node, int existingIndex)
    {
        if (node.StepsFromStart < _next[existingIndex].StepsFromStart)
        {
            _next[existingIndex] = node;
        }
    }

    private Node Dequeue(Comparison<Node> comparer)
    {
        if (_next.Count == 0)
            return null;

        _next.Sort(comparer);
        Node node = _next[0];
        _next.RemoveAt(0);
        return node;
    }

    private int FindNodeInNext(Vector2Int cell)
    {
        return _next.FindIndex(n => n.Cell == cell);
    }

    private List<Vector2Int> ReconstructLimitedPath(Node node)
    {
        _path.Clear();
        while (node != null)
        {
            if (node.StepsFromStart <= _character.TraversalRange)
            {
                _path.Add(node.Cell);
            }
            node = node.Parent;
        }
        _path.Reverse();
        return _path;
    }
}