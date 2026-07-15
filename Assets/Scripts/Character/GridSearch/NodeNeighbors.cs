using System.Collections;
using System.Collections.Generic;

public class NodeNeighbors : IEnumerable<Node>
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

    public IEnumerator<Node> GetEnumerator()
    {
        yield return Up;
        yield return Right;
        yield return Down;
        yield return Left;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}