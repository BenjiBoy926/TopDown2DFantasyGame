using UnityEngine;

public struct CellNeighbors
{
    public Vector2Int Left, Right, Up, Down;

    public static CellNeighbors Get(Vector2Int center)
    {
        return new CellNeighbors
        {
            Left = center + Vector2Int.left,
            Right = center + Vector2Int.right,
            Up = center + Vector2Int.up,
            Down = center + Vector2Int.down
        };
    }
}