using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Grid))]
public class Battlefield : MonoBehaviour
{
    public float CellWidth => _grid.cellSize.x;
    public float CellHeight => _grid.cellSize.y;

    private Grid _grid;
    private readonly Dictionary<Vector2Int, Character> _cellToOccupant = new();
    private readonly Dictionary<Character, Vector2Int> _occupantToCell = new();

    public static int RectangularDistance(Vector2Int a, Vector2Int b)
    {
        Vector2Int offset = b - a;
        Vector2Int absoluteOffset = new(Mathf.Abs(offset.x), Mathf.Abs(offset.y));
        return absoluteOffset.x + absoluteOffset.y;
    }

    public void Register(Character character)
    {
        character.Position = SnapToGrid(character.Position);
        Vector2Int cell = WorldToCell(character.Position);
        _occupantToCell[character] = cell;
        _cellToOccupant[cell] = character;
    }

    public void Unregister(Character character)
    {
        Vector2Int cell = _occupantToCell[character];
        _occupantToCell.Remove(character);
        _cellToOccupant.Remove(cell);
    }

    public Vector2 SnapToGrid(Vector2 position)
    {
        Vector2Int cell = WorldToCell(position);
        return CellToWorld(cell);
    }

    public Vector2 CellToWorld(Vector2Int cell)
    {
        return _grid.CellToWorld((Vector3Int)cell);
    }

    public Vector2Int WorldToCell(Vector2 position)
    {
        // NOTE: WorldToCell uses FloorToInt, not RoundToInt,
        // but if we offset the original position we can get the same result as Round
        // without breaking non-rectangular cell shapes (maybe? haven't tested it)
        position += (Vector2)_grid.cellSize * .5f;
        return (Vector2Int)_grid.WorldToCell(position);
    }

    public Character GetOccupant(Vector2Int cell)
    {
        return _cellToOccupant.TryGetValue(cell, out Character character) ? character : null;
    }

    public Vector2Int GetCell(Character character)
    {
        return _occupantToCell[character];
    }

    public void RefreshOccupantCell(Character character)
    {
        Vector2Int oldCell = _occupantToCell[character];
        Vector2Int newCell = WorldToCell(character.Position);

        _cellToOccupant.Remove(oldCell);
        _cellToOccupant[newCell] = character;
        _occupantToCell[character] = newCell;
    }

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }
}
