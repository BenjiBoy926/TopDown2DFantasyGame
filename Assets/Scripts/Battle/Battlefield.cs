using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[RequireComponent(typeof(Battle))]
[RequireComponent(typeof(Tilemap))]
public class Battlefield : MonoBehaviour
{
    public float CellWidth => _tilemap.cellSize.x;
    public float CellHeight => _tilemap.cellSize.y;

    private Rect _area;
    private Tilemap _tilemap;
    private readonly Dictionary<Vector2Int, Character> _cellToOccupant = new();
    private readonly Dictionary<Character, Vector2Int> _occupantToCell = new();
    
    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();

        _tilemap.CompressBounds();
        BoundsInt cellBounds = _tilemap.cellBounds;
        Vector2 minWorldPosition = CellToWorld((Vector2Int)cellBounds.min);
        Vector2 maxWorldPosition = CellToWorld((Vector2Int)cellBounds.max);
        _area = Rect.MinMaxRect(minWorldPosition.x, minWorldPosition.y, maxWorldPosition.x, maxWorldPosition.y);
    }

    public void Register(Character character)
    {
        Vector2Int cell = WorldToCell(character.Position);
        _cellToOccupant[cell] = character;
        _occupantToCell[character] = cell;
    }

    public void Unregister(Character character)
    {
        if (_occupantToCell.TryGetValue(character, out Vector2Int cell))
        {
            _occupantToCell.Remove(character);
            _cellToOccupant.Remove(cell);
        }
    }

    public void RefreshCell(Character character)
    {
        Vector2Int oldCell = _occupantToCell[character];
        Vector2Int newCell = WorldToCell(character.Position);

        _cellToOccupant.Remove(oldCell);
        _cellToOccupant[newCell] = character;
        _occupantToCell[character] = newCell;
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

        // NOTE: this casting is a little bit expensive, consider revising?
        position += (Vector2)_tilemap.cellSize * .5f;
        return (Vector2Int)_tilemap.WorldToCell(position);
    }

    public Character GetOccupant(Vector2Int cell)
    {
        return _cellToOccupant.ContainsKey(cell) ? _cellToOccupant[cell] : null;
    }

    public Vector2Int GetCell(Character occupant)
    {
        return _occupantToCell[occupant];
    }

    public TileBase GetTile(Vector2Int cell)
    {
        return _tilemap.GetTile((Vector3Int)cell);
    }

    public Vector2 Clamp(Vector2 position)
    {
        position.x = Mathf.Clamp(position.x, _area.xMin, _area.xMax);
        position.y = Mathf.Clamp(position.y, _area.yMin, _area.yMax);
        return position;
    }
}
