using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

public class ComputerPlayerPathfinder : MonoBehaviour
{
    public class Node
    {
        public int Cost => Parent != null ? Parent.Cost + 1 : 0;

        public Vector2Int Cell;
        public Node Parent;
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
    private Vector2Int _target;

    private static readonly HashSet<Node> _visited = new();
    private static readonly List<Node> _next = new();

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
                // TODO return actual path
                return new();
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
        return !_visited.Contains(node) && IsTraversible(node);
    }

    private bool IsTraversible(Node node)
    {
        return node.Cost <= _character.TraversalRange && _character.IsPassable(node.Cell);
    }

    private void Enqueue(Node node)
    {
        _next.Add(node);
    }

    private Node Dequeue()
    {
        if (_next.Count == 0)
            return null;

        Node node = _next[0];
        _next.RemoveAt(0);
        return node;
    }
}