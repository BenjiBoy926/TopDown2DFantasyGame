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

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
    }

    public void Refresh()
    {
        Clear();
        foreach (var stayableCell in _character.StayableCells)
        {
            SpriteRenderer cell = AddCell(stayableCell);
            cell.color = _stayableCellColor;
        }
        foreach (var interactableEdgeCell in _character.InteractableEdgeCells)
        {
            SpriteRenderer cell = AddCell(interactableEdgeCell);
            cell.color = GetCellColor(interactableEdgeCell);
        }
        ReflectCurrentAlpha();
    }


    public void Hide()
    {
        SetCurrentAlpha(0);
    }

    public void ShowTransparent()
    {
        SetCurrentAlpha(_transparentAlpha);
    }

    public void ShowOpaque()
    {
        SetCurrentAlpha(_opaqueAlpha);
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
        SpriteRenderer cellObj = Instantiate(_cellPrefab, position, Quaternion.identity);
        _cells.Add(cellObj);
        return cellObj;
    }

    private Color GetCellColor(Vector2Int cell)
    {
        Obstacle obstacle = _character.GetObstacle(cell);
        if (!obstacle || obstacle == _character.Obstacle)
        {
            return _stayableCellColor;
        }
        else if (obstacle.Faction == _character.Faction)
        {
            return _allyInteractionCellColor;
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
