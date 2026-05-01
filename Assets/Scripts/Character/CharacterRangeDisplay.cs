using System.Collections.Generic;
using UnityEngine;

public class CharacterRangeDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _cellPrefab;
    [SerializeField] private Color _stayableCellColor = Color.blue;
    [SerializeField] private Color _attackableCellColor = Color.red;
    [SerializeField] private Color _allyInteractionCellColor = Color.green;
    [SerializeField] private float _transparentAlpha = 0.2f;
    [SerializeField] private float _opaqueAlpha = 0.5f;
    private Character _character;
    private readonly HashSet<SpriteRenderer> _cells = new();
    private float _currentAlpha;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
    }

    public void Refresh()
    {
        // Create cells
    }

    public void Hide()
    {
        // Set alpha to 0
    }

    public void ShowTransparent()
    {

    }

    public void ShowOpaque()
    {

    }

    private void AddCell(Vector2Int cell)
    {
        Vector2 position = _character.CellToWorld(cell);
        SpriteRenderer cellObj = Instantiate(_cellPrefab, position, Quaternion.identity);
        _cells.Add(cellObj);
    }
}
