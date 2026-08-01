using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharacterRangeDisplay))]
public class CharacterRange : MonoBehaviour
{
    private const float ClampMargin = 0.1f;

    public IReadOnlyCollection<Vector2Int> StayableCells => _stayableCells;
    public IReadOnlyCollection<Vector2Int> EdgeCells => _edgeCells;
    public IReadOnlyCollection<Vector2Int> ReachableCells => _reachableCells;
    public IReadOnlyCollection<Vector2Int> AllCells => _allCells;

    [SerializeField] private List<TileBase> _wallTiles = new();
    private Character _character;
    private CharacterRangeDisplay _display;
    private readonly HashSet<Vector2Int> _traversibleCells = new();
    private readonly HashSet<Vector2Int> _stayableCells = new();
    private readonly HashSet<Vector2Int> _edgeCells = new();
    private readonly HashSet<Vector2Int> _reachableCells = new();
    private readonly HashSet<Vector2Int> _allCells = new();

    private void Awake()
    {
        _character = GetComponent<Character>();
        _display = GetComponent<CharacterRangeDisplay>();
    }

    public void Refresh()
    {
        RecalculateTraversibleCells();
        RecalculateStayableCells();
        RecalculateEdgeCells();
        RecalculateReachableCells();
        RecalculateAllCells();
        _display.Refresh();
    }

    public void ShowTransparentRange()
    {
        _display.ShowTransparent();
    }

    public void ShowOpaqueRange()
    {
        _display.ShowOpaque();
    }

    public void Hide()
    {
        _display.Hide();
    }

    private void RecalculateTraversibleCells()
    {
        GridSearchResult result = _character.SearchGrid(new GridSearchStrategy.FindAllCellsInRange());
        _traversibleCells.Clear();
        foreach (var cell in result.VisitedCells)
        {
            _traversibleCells.Add(cell);
        }
    }

    private void RecalculateStayableCells()
    {
        _stayableCells.Clear();
        foreach (var traversibleCell in _traversibleCells)
        {
            if (_character.CouldStayInCell(traversibleCell))
            {
                _stayableCells.Add(traversibleCell);
            }
        }
    }

    private void RecalculateEdgeCells()
    {
        _edgeCells.Clear();
        foreach (var traversibleCell in _traversibleCells)
        {
            CellNeighbors neighbors = CellNeighbors.Get(traversibleCell);
            foreach (var neighborCell in neighbors)
            {
                if (!_stayableCells.Contains(neighborCell))
                {
                    _edgeCells.Add(neighborCell);
                }
            }
        }
    }

    private void RecalculateReachableCells()
    {
        _reachableCells.Clear();
        foreach (var stayableCell in _stayableCells)
        {
            CellNeighbors neighbors = CellNeighbors.Get(stayableCell);
            _reachableCells.UnionWith(neighbors);
        }
    }

    private void RecalculateAllCells()
    {
        _allCells.Clear();
        _allCells.UnionWith(_traversibleCells);
        _allCells.UnionWith(_edgeCells);
    }

    public Vector2 ClampToStayableCells(Vector2 position)
    {
        return ClampToCells(position, _stayableCells);
    }

    public Vector2 ClampToReachableCells(Vector2 position)
    {
        return ClampToCells(position, _reachableCells);
    }

    public bool CouldWalkThroughCell(Vector2Int cell)
    {
        TileBase tile = _character.GetTile(cell);
        return tile && !_wallTiles.Contains(tile) && !_character.IsEnemyInCell(cell, out _);
    }

    public bool Contains(Vector2Int cell)
    {
        return _allCells.Contains(cell);
    }

    public bool IsReachable(Vector2Int cell)
    {
        return _reachableCells.Contains(cell);
    }

    public bool IsStayable(Vector2Int cell)
    {
        return _stayableCells.Contains(cell);
    }

    private Vector2 ClampToCells(Vector2 position, HashSet<Vector2Int> cells)
    {
        Vector2Int currentCell = _character.WorldToCell(position);
        if (cells.Contains(currentCell)) return position;

        // NOTE: min rect distance is 1 but should be higher for characters with a wider attack range
        Vector2Int closestCell = ClosestCell(position, cells);
        Vector2 cellPosition = _character.CellToWorld(closestCell);

        float xExtent = _character.CellWidth / 2 - (ClampMargin * 2);
        float yExtent = _character.CellHeight / 2 - (ClampMargin * 2);
        Rect range = Rect.MinMaxRect(cellPosition.x - xExtent, cellPosition.y - yExtent, cellPosition.x + xExtent, cellPosition.y + yExtent);

        float x = Mathf.Clamp(position.x, range.xMin, range.xMax);
        float y = Mathf.Clamp(position.y, range.yMin, range.yMax);

        return new(x, y);
    }

    private Vector2Int ClosestCell(Vector2 input, HashSet<Vector2Int> cells)
    {
        Vector2Int inputCell = _character.WorldToCell(input);
        if (cells.Count == 0) return inputCell;

        Vector2Int closestCell = Vector2Int.zero;
        int closestRectangularDistance = int.MaxValue;
        float closestEuclidianDistance = float.MaxValue;
        foreach (Vector2Int cell in cells)
        {
            int rectangularDistance = RectangularDistance(inputCell, cell);
            float euclidianDistance = SqrDistance(input, cell);

            if (rectangularDistance < closestRectangularDistance ||
                (rectangularDistance == closestRectangularDistance && euclidianDistance < closestEuclidianDistance))
            {
                closestCell = cell;
                closestRectangularDistance = rectangularDistance;
                closestEuclidianDistance = euclidianDistance;
            }
        }
        return closestCell;
    }

    private float SqrDistance(Vector2 position, Vector2Int cell)
    {
        Vector2 cellPosition = _character.CellToWorld(cell);
        return (position - cellPosition).sqrMagnitude;
    }

    public static int RectangularDistance(Vector2Int a, Vector2Int b)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);
        return xDist + yDist;
    }
}
