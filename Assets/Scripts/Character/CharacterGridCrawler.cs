using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterGridCrawler : MonoBehaviour
{
    public class Node
    {
        public int DistanceFromStart => Parent != null ? Parent.DistanceFromStart + 1 : 0;

        public Vector2Int Cell;
        public Node Parent;

        public int GetCost(Vector2Int target)
        {
            return DistanceFromStart + GetEstimatedDistanceToEnd(target);
        }

        public int GetEstimatedDistanceToEnd(Vector2Int target)
        {
            return CharacterRange.RectangularDistance(Cell, target);
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

    private Character _character;

    private static readonly HashSet<Node> _visited = new();
    private static readonly List<Node> _next = new();
    private static readonly List<Vector2Int> _path = new();

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public List<Vector2Int> FindPath(Vector2Int target)
    {
        int CompareNodes(Node a, Node b)
        {
            int aCost = a.GetCost(target);
            int bCost = b.GetCost(target);
            return aCost.CompareTo(bCost);
        }

        Node targetNode = null;
        void SortBeforeDequeue() => _next.Sort(CompareNodes);
        bool ExitCondition(Node node) => node.Cell == target;
        void ExitAction(Node node) => targetNode = node;
        Crawl(SortBeforeDequeue, ExitCondition, ExitAction);
        return ReconstructPath(targetNode);
    }

    public void Crawl() 
    {
        Crawl(null, null, null);
    }

    private void Crawl(Action sortBeforeDequeue, Predicate<Node> exitCondition, Action<Node> exitAction)
    {
        _visited.Clear();
        _next.Clear();

        Node start = new() { Cell = _character.CurrentCell, Parent = null };
        Enqueue(start);

        while (_next.Count > 0)
        {
            sortBeforeDequeue?.Invoke();
            Node current = Dequeue();
            _visited.Add(current);

            if (exitCondition?.Invoke(current) == true)
            {
                exitAction?.Invoke(current);
                return;
            }

            VisitNeighbors(current);
        }
    }

    private void VisitNeighbors(Node node)
    {
        NodeNeighbors neighbors = NodeNeighbors.Get(node);
        Visit(neighbors.Left);
        Visit(neighbors.Right);
        Visit(neighbors.Up);
        Visit(neighbors.Down);
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
        return IsTraversible(node);
    }

    private bool IsTraversible(Node node)
    {
        return node.DistanceFromStart <= _character.TraversalRange && _character.IsPassable(node.Cell);
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

    private static void UpdateExistingNode(Node node, int existingIndex)
    {
        if (node.DistanceFromStart < _next[existingIndex].DistanceFromStart)
        {
            _next[existingIndex] = node;
        }
    }

    private Node Dequeue()
    {
        if (_next.Count == 0)
            return null;

        Node node = _next[0];
        _next.RemoveAt(0);
        return node;
    }

    private int FindNodeInNext(Vector2Int cell)
    {
        return _next.FindIndex(n => n.Cell == cell);
    }

    private List<Vector2Int> ReconstructPath(Node node)
    {
        _path.Clear();
        while (node != null)
        {
            _path.Add(node.Cell);
            node = node.Parent;
        }
        _path.Reverse();
        return _path;
    }
}