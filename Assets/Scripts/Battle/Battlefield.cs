using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[RequireComponent(typeof(Tilemap))]
public class Battlefield : MonoBehaviour
{
    public float CellWidth => _tilemap.cellSize.x;
    public float CellHeight => _tilemap.cellSize.y;

    private Tilemap _tilemap;
    private readonly Dictionary<Vector2Int, Character> _cellToCharacter = new();
    private readonly Dictionary<Character, Vector2Int> _characterToCell = new();

    public void Register(Character character)
    {
        character.Position = SnapToGrid(character.Position);
        Vector2Int cell = WorldToCell(character.Position);
        _characterToCell[character] = cell;
        _cellToCharacter[cell] = character;
    }

    public void Unregister(Character character)
    {
        Vector2Int cell = _characterToCell[character];
        _characterToCell.Remove(character);
        _cellToCharacter.Remove(cell);
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

    public Character GetCharacter(Vector2Int cell)
    {
        return _cellToCharacter.TryGetValue(cell, out Character character) ? character : null;
    }

    public Vector2Int GetCell(Character character)
    {
        return _characterToCell[character];
    }

    public void RefreshCell(Character character)
    {
        Vector2Int oldCell = _characterToCell[character];
        Vector2Int newCell = WorldToCell(character.Position);

        _cellToCharacter.Remove(oldCell);
        _cellToCharacter[newCell] = character;
        _characterToCell[character] = newCell;
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
