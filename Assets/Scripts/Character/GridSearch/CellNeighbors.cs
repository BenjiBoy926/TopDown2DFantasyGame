using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CellNeighbors
{
    public const int Count = 4;

    public readonly Vector2Int this[int index] => index switch
    {
        0 => Up,
        1 => Right,
        2 => Down,
        3 => Left,
        _ => throw new System.IndexOutOfRangeException()
    };

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