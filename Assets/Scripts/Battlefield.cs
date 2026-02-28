using UnityEngine;

[RequireComponent(typeof(Grid))]
public class Battlefield : MonoBehaviour
{
    private Grid _grid;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        // NOTE: WorldToCell uses FloorToInt, not RoundToInt,
        // but if we offset the original position we can get the same result as Round
        // without breaking non-rectangular cell shapes
        position += _grid.cellSize * .5f;
        Vector3Int cell = _grid.WorldToCell(position);
        return _grid.CellToWorld(cell);
    }
}
