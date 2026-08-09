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
    private float _currentAlpha = 0;

    private static Transform _cellParent;
    private static int _instanceCount = 0;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _instanceCount++;
        if (_instanceCount == 1)
        {
            _cellParent = new GameObject($"Cell Highlight Parent").transform;
        }
    }

    private void OnDestroy()
    {
        _instanceCount--;
        if (_instanceCount == 0)
        {
            Destroy(_cellParent.gameObject);
        }
    }

    public void Hide()
    {
        Clear();
    }

    public void ShowTransparent()
    {
        Refresh();
        SetCurrentAlpha(_transparentAlpha);
    }

    public void ShowOpaque()
    {
        Refresh();
        SetCurrentAlpha(_opaqueAlpha);
    }

    private void Refresh()
    {
        Clear();
        foreach (var cell in _character.AllCellsInRange)
        {
            AddCell(cell);
        }
    }

    private void Clear()
    {
        foreach (var cell in _cells)
        {
            Destroy(cell.gameObject);
        }
        _cells.Clear();
    }

    private SpriteRenderer AddCell(Vector2Int cell)
    {
        Vector2 position = _character.CellToWorld(cell);
        SpriteRenderer cellObj = Instantiate(_cellPrefab, position, Quaternion.identity, _cellParent);
        cellObj.color = GetCellColor(cell);
        _cells.Add(cellObj);
        return cellObj;
    }

    private Color GetCellColor(Vector2Int cell)
    {
        if (_character.IsAllyInCell(cell))
        {
            return _allyInteractionCellColor;
        }
        else if (_character.IsStayable(cell))
        {
            return _stayableCellColor;
        }
        else
        {
            return _attackableCellColor;
        }
    }

    private void SetCurrentAlpha(float alpha)
    {
        _currentAlpha = alpha;
        ReflectCurrentAlpha();
    }

    private void ReflectCurrentAlpha()
    {
        foreach (var cell in _cells)
        {
            Color color = cell.color;
            color.a = _currentAlpha;
            cell.color = color;
        }
    }
}
