using UnityEngine;

public readonly struct CellCost
{
    public readonly Vector2Int Cell;
    public readonly int CostToArrive;

    public CellCost(Vector2Int cell, int costToArrive)
    {
        Cell = cell;
        CostToArrive = costToArrive;
    }
}