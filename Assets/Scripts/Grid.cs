using System.Collections;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private float _cellSize = 2;

    public Vector2 SnapWorldPositionToGrid(Vector2 worldPosition)
    {
        Vector2Int gridPosition = GetGridPosition(worldPosition);
        return GetWorldPosition(gridPosition);
    }

    public Vector2 GetWorldPosition(Vector2Int gridPosition)
    {
        return new(gridPosition.x * _cellSize, gridPosition.y * _cellSize);
    }

    public Vector2Int GetGridPosition(Vector2 worldPosition)
    {
        Vector2 shrunkPosition = worldPosition / _cellSize;
        int x = Mathf.RoundToInt(shrunkPosition.x);
        int y = Mathf.RoundToInt(shrunkPosition.y);
        return new(x, y);
    }
}
