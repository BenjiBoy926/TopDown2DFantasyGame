
public readonly struct CellCostNeighbors
{
    public readonly CellCost Left, Right, Up, Down;

    public CellCostNeighbors(CellCost left, CellCost right, CellCost up, CellCost down)
    {
        Left = left;
        Right = right;
        Up = up;
        Down = down;
    }

    public static CellCostNeighbors Get(CellCost center)
    {
        CellNeighbors neighbors = CellNeighbors.Get(center.Cell);
        return new(
            new CellCost(neighbors.Left, center.CostToArrive + 1),
            new CellCost(neighbors.Right, center.CostToArrive + 1),
            new CellCost(neighbors.Up, center.CostToArrive + 1),
            new CellCost(neighbors.Down, center.CostToArrive + 1));
    }
}