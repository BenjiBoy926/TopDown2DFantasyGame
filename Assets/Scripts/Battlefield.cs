using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Grid))]
public class Battlefield : MonoBehaviour
{
    private Grid _grid;
    private readonly Dictionary<Vector3Int, Character> _cellToOccupant = new();
    private readonly Dictionary<Character, Vector3Int> _occupantToCell = new();

    public void Register(Character character)
    {
        character.Position = SnapToGrid(character.Position);
        Vector3Int cell = WorldToCellRounded(character.Position);
        _occupantToCell[character] = cell;
        _cellToOccupant[cell] = character;
    }

    public void Unregister(Character character)
    {
        Vector3Int cell = _occupantToCell[character];
        _occupantToCell.Remove(character);
        _cellToOccupant.Remove(cell);
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        Vector3Int cell = WorldToCellRounded(position);
        return _grid.CellToWorld(cell);
    }

    public Vector3Int WorldToCellRounded(Vector3 position)
    {
        // NOTE: WorldToCell uses FloorToInt, not RoundToInt,
        // but if we offset the original position we can get the same result as Round
        // without breaking non-rectangular cell shapes (maybe? haven't tested it)
        position += _grid.cellSize * .5f;
        return _grid.WorldToCell(position);
    }

    public Character GetOccupant(Vector3Int cell)
    {
        return _cellToOccupant.TryGetValue(cell, out Character character) ? character : null;
    }

    public void RefreshOccupantCell(Character character)
    {
        Vector3Int oldCell = _occupantToCell[character];
        Vector3Int newCell = WorldToCellRounded(character.Position);

        _cellToOccupant.Remove(oldCell);
        _cellToOccupant[newCell] = character;
        _occupantToCell[character] = newCell;
    }

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }
}
