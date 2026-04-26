using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[RequireComponent(typeof(Tilemap))]
public class Battlefield : MonoBehaviour
{
    public float CellWidth => _tilemap.cellSize.x;
    public float CellHeight => _tilemap.cellSize.y;

    private Tilemap _tilemap;
    private readonly Dictionary<Vector2Int, Obstacle> _cellToObstacle = new();
    private readonly Dictionary<Obstacle, Vector2Int> _obstacleToCell = new();

    public void Register(Obstacle obstacle)
    {
        obstacle.Position = SnapToGrid(obstacle.Position);
        Vector2Int cell = WorldToCell(obstacle.Position);
        _obstacleToCell[obstacle] = cell;
        _cellToObstacle[cell] = obstacle;
    }

    public void Unregister(Obstacle obstacle)
    {
        Vector2Int cell = _obstacleToCell[obstacle];
        _obstacleToCell.Remove(obstacle);
        _cellToObstacle.Remove(cell);
    }

    public Vector2 SnapToGrid(Vector2 position)
    {
        Vector2Int cell = WorldToCell(position);
        return CellToWorld(cell);
    }

    public Vector2 CellToWorld(Vector2Int cell)
    {
        return _tilemap.CellToWorld((Vector3Int)cell);
    }

    public Vector2Int WorldToCell(Vector2 position)
    {
        // NOTE: WorldToCell uses FloorToInt, not RoundToInt,
        // but if we offset the original position we can get the same result as Round
        // without breaking non-rectangular cell shapes (maybe? haven't tested it)
        position += (Vector2)_tilemap.cellSize * .5f;
        return (Vector2Int)_tilemap.WorldToCell(position);
    }

    public Obstacle GetObstacle(Vector2Int cell)
    {
        return _cellToObstacle.TryGetValue(cell, out Obstacle obstacle) ? obstacle : null;
    }

    public Vector2Int GetCell(Obstacle obstacle)
    {
        return _obstacleToCell[obstacle];
    }

    public void RefreshCell(Obstacle obstacle)
    {
        Vector2Int oldCell = _obstacleToCell[obstacle];
        Vector2Int newCell = WorldToCell(obstacle.Position);

        _cellToObstacle.Remove(oldCell);
        _cellToObstacle[newCell] = obstacle;
        _obstacleToCell[obstacle] = newCell;
    }

    public TileBase GetTile(Vector2Int cell)
    {
        return _tilemap.GetTile((Vector3Int)cell);
    }

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }
}
