using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterRangeDisplay : MonoBehaviour
{
    public bool IsShown => _isShown;

    [SerializeField] private GameObject _traversibleCellPrefab;
    [SerializeField] private GameObject _attackableCellPrefab;
    private Character _character;
    private readonly HashSet<GameObject> _cells = new();
    private bool _isShown;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void Show()
    {
        foreach (Vector2Int cell in _character.TraversibleCells)
        {
            AddCell(_traversibleCellPrefab, cell);
        }
        foreach (Vector2Int cell in _character.AttackableEdgeCells)
        {
            AddCell(_attackableCellPrefab, cell);
        }
        _isShown = true;
    }

    public void Hide()
    {
        foreach (GameObject cell in _cells)
        {
            Destroy(cell);
        }
        _cells.Clear();
        _isShown = false;
    }

    private void AddCell(GameObject prefab, Vector2Int cell)
    {
        Vector2 position = _character.CellToWorld(cell);
        GameObject cellObj = Instantiate(prefab, position, Quaternion.identity);
        _cells.Add(cellObj);
    }
}
