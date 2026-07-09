using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayerPathfinder : MonoBehaviour
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

    [SerializeField] private float _speed = 2;

    private Character _character;
    private Vector2Int _target;

    private static readonly HashSet<Node> _visited = new();
    private static readonly List<Node> _next = new();
    private static readonly List<Vector2Int> _path = new();

    public IEnumerator MoveToCell(Character character, Vector2Int cell)
    {
        List<Vector2Int> path = FindPath(character, cell);
        character.SetIsRunning(true);
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int nextCell = path[i];
            yield return MoveDirectlyToCell(character, nextCell);
        }
        character.SetIsRunning(false);
    }

    private YieldInstruction MoveDirectlyToCell(Character character, Vector2Int cell)
    {
        Vector2 nextPosition = character.CellToWorld(cell);
        return character.transform.DOMove(nextPosition, _speed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }

    public List<Vector2Int> FindPath(Character character, Vector2Int target)
    {
        _character = character;
        _target = target;

        _visited.Clear();
        _next.Clear();

        Node start = new() { Cell = character.CurrentCell, Parent = null };
        Enqueue(start);

        while (_next.Count > 0)
        {
            Node current = Dequeue();
            _visited.Add(current);

            if (current.Cell == target)
            {
                return ReconstructPath(current);
            }

            VisitNeighbors(current);
        }

        return new();
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
            if (node.DistanceFromStart < _next[existingIndex].DistanceFromStart)
            {
                _next[existingIndex] = node;
            }
        }
        else
        {
            _next.Add(node);
        }
    }

    private Node Dequeue()
    {
        if (_next.Count == 0)
            return null;

        _next.Sort(CompareNodes);
        Node node = _next[0];
        _next.RemoveAt(0);
        return node;
    }

    private int FindNodeInNext(Vector2Int cell)
    {
        return _next.FindIndex(n => n.Cell == cell);
    }

    private int CompareNodes(Node a, Node b)
    {
        int aCost = a.GetCost(_target);
        int bCost = b.GetCost(_target);
        return aCost.CompareTo(bCost);
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