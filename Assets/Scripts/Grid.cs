using System.Collections;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private float _cellSize = 2;

    public Vector2 SnapToGrid(Vector2 worldPosition)
    {
        Vector2Int gridPosition = GetGridCellIndex(worldPosition);
        return GetWorldPositionOfCell(gridPosition);
    }

    public Vector2 GetWorldPositionOfCell(Vector2Int gridPosition)
    {
        return new(gridPosition.x * _cellSize, gridPosition.y * _cellSize);
    }

    public Vector2Int GetGridCellIndex(Vector2 worldPosition)
    {
        Vector2 shrunkPosition = worldPosition / _cellSize;
        int x = Mathf.RoundToInt(shrunkPosition.x);
        int y = Mathf.RoundToInt(shrunkPosition.y);
        return new(x, y);
    }
}
