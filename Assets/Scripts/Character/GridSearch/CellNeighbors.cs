using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CellNeighbors : IEnumerable<Vector2Int>
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

    public readonly IEnumerator<Vector2Int> GetEnumerator()
    {
        yield return Up;
        yield return Right;
        yield return Down;
        yield return Left;
    }

    readonly IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}