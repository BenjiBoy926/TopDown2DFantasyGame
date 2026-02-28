using UnityEngine;

public static class GridExtensions
{
    public static Vector3 Round(this Grid grid, Vector3 position)
    {
        // NOTE: WorldToCell uses FloorToInt, not RoundToInt,
        // but if we offset the original position we can get the same result as Round
        // without breaking non-rectangular cell shapes
        position += grid.cellSize * .5f;
        Vector3Int cell = grid.WorldToCell(position);
        return grid.CellToWorld(cell);
    }
}
