using System.Collections.Generic;
using UnityEngine;

public class CharacterRangeDisplay : MonoBehaviour
{
    public bool IsShown => _isShown;

    [SerializeField] private GameObject _tilePrefab;
    private readonly HashSet<GameObject> _tiles = new();
    private bool _isShown;

    public void Show(Character character)
    {
        foreach (Vector3Int tile in character.TraversibleTiles)
        {
            Vector3 position = character.CellToWorld(tile);
            GameObject tileObj = Instantiate(_tilePrefab, position, Quaternion.identity);
            _tiles.Add(tileObj);
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
}
