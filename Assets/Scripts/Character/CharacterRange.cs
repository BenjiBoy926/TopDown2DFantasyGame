using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharacterRangeDisplay))]
[RequireComponent(typeof(CharacterGridCrawler))]
public class CharacterRange : MonoBehaviour
{
    private const float ClampMargin = 0.1f;

    public IReadOnlyCollection<Vector2Int> StayableCells => _stayableCells;
    public IReadOnlyCollection<Vector2Int> InteractableEdgeCells => _interactableEdgeCells;

    [SerializeField] private List<TileBase> _wallTiles = new();
    private Character _character;
    private CharacterRangeDisplay _display;
    private CharacterGridCrawler _gridCrawler;
    private readonly HashSet<Vector2Int> _traversibleCells = new();
    private readonly HashSet<Vector2Int> _stayableCells = new();
    private readonly HashSet<Vector2Int> _interactableEdgeCells = new();
    private readonly HashSet<Vector2Int> _reachableCells = new();

    private void Awake()
    {
        _character = GetComponent<Character>();
        _display = GetComponent<CharacterRangeDisplay>();
        _gridCrawler = GetComponent<CharacterGridCrawler>();
    }

    public List<Vector2Int> FindPath(Vector2Int target)
    {
        return _gridCrawler.FindPath(target);
    }

    public void Refresh()
    {
        RecalculateTraversibleCells();
        RecalculateStayableCells();
        RecalculateAttackableEdgeCells();

        _reachableCells.Clear();
        _reachableCells.UnionWith(_traversibleCells);
        _reachableCells.UnionWith(_interactableEdgeCells);
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
        _gridCrawler.FindAllCellsInRange();
        _traversibleCells.Clear();
        foreach (var visitedNode in _gridCrawler.Visited)
        {
            _traversibleCells.Add(visitedNode.Cell);
        }
    }

    private void RecalculateStayableCells()
    {
        _stayableCells.Clear();
        foreach (var traversibleCell in _traversibleCells)
        {
            if (_character.CanStayInCell(traversibleCell))
            {
                _stayableCells.Add(traversibleCell);
            }
        }
    }

    private void RecalculateAttackableEdgeCells()
    {
        _interactableEdgeCells.Clear();
        foreach (var stayableCell in _stayableCells)
        {
            CellNeighbors neighbors = CellNeighbors.Get(stayableCell);
            CheckAttackableEdgeCell(neighbors.Left);
            CheckAttackableEdgeCell(neighbors.Right);
            CheckAttackableEdgeCell(neighbors.Up);
            CheckAttackableEdgeCell(neighbors.Down);
        }
    }

    public Vector2 ClampToStayableCells(Vector2 position)
    {
        return ClampToCells(position, _stayableCells);
    }

    public Vector2 ClampToReachableCells(Vector2 position)
    {
        return ClampToCells(position, _reachableCells);
    }

    public bool IsPassable(Vector2Int cell)
    {
        TileBase tile = _character.GetTile(cell);
        if (tile && _wallTiles.Contains(tile))
        {
            return false;
        }

        Character occupant = _character.GetOccupant(cell);
        bool canMoveThroughOccupant = !occupant || occupant.Faction == _character.Faction;
        return canMoveThroughOccupant;
    }

    public bool IsReachable(Vector2Int cell)
    {
        return _reachableCells.Contains(cell);
    }

    public bool IsStayable(Vector2Int cell)
    {
        return _stayableCells.Contains(cell);
    }

    private void CheckAttackableEdgeCell(Vector2Int cell)
    {
        if (!_stayableCells.Contains(cell))
        {
            _interactableEdgeCells.Add(cell);
        }
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
