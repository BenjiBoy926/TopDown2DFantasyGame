using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[RequireComponent(typeof(Battle))]
[RequireComponent(typeof(Tilemap))]
public class Battlefield : MonoBehaviour
{
    public float CellWidth => _tilemap.cellSize.x;
    public float CellHeight => _tilemap.cellSize.y;

    private Battle _battle;
    private Tilemap _tilemap;
    
    private void Awake()
    {
        _battle = GetComponent<Battle>();
        _tilemap = GetComponent<Tilemap>();
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

    // ALLOCATES: change to concrete type
    public Character GetOccupant(Vector2Int cell)
    {
        foreach (var character in _battle.AllCharacters)
        {
            if (character.CurrentCell == cell)
            {
                return character;
            }
        }
        return null;
    }

    public TileBase GetTile(Vector2Int cell)
    {
        return _tilemap.GetTile((Vector3Int)cell);
    }
}
