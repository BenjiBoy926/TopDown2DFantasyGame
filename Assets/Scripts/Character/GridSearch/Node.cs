using UnityEngine;

public class Node
{
    public int StepsFromStart => Parent != null ? Parent.StepsFromStart + 1 : 0;

    public Vector2Int Cell;
    public Node Parent;

    public int GetPathfindingCost(Vector2Int target)
    {
        return StepsFromStart + GetEstimatedStepsToEnd(target);
    }

    public int GetEstimatedStepsToEnd(Vector2Int target)
    {
        return CharacterRange.RectangularDistance(Cell, target);
    }
}
