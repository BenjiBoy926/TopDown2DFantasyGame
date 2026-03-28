using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterRangeDisplay : MonoBehaviour
{
    public bool IsShown => _isShown;

    [SerializeField] private GameObject _traversibleTilePrefab;
    [SerializeField] private GameObject _attackableTilePrefab;
    private Character _character;
    private readonly HashSet<GameObject> _tiles = new();
    private bool _isShown;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void Show()
    {
        foreach (Vector3Int tile in _character.TraversibleTiles)
        {
            AddTile(_traversibleTilePrefab, tile);
        }
        foreach (Vector3Int tile in _character.AttackableEdgeTiles)
        {
            AddTile(_attackableTilePrefab, tile);
        }
        _isShown = true;
    }

    public void Hide()
    {
        foreach (GameObject tile in _tiles)
        {
            Destroy(tile);
        }
        _tiles.Clear();
        _isShown = false;
    }

    private void AddTile(GameObject prefab, Vector3Int tile)
    {
        Vector3 position = _character.CellToWorld(tile);
        GameObject tileObj = Instantiate(prefab, position, Quaternion.identity);
        _tiles.Add(tileObj);
    }
}
