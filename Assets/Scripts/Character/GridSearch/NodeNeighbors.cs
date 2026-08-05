public struct NodeNeighbors
{
    public const int Count = 4;

    public readonly Node this[int index] => index switch
    {
        0 => Up,
        1 => Right,
        2 => Down,
        3 => Left,
        _ => throw new System.IndexOutOfRangeException()
    };

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