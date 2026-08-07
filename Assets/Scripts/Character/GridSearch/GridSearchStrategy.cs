using System;
using UnityEngine;

public abstract class GridSearchStrategy
{
    public abstract bool IsExitNode(GridSearchState state, Node node);
    public abstract bool PassesCustomEnqueueConditions(GridSearchState state, Node node);
    public abstract int GetNodeCost(GridSearchState state, Node node);

    public class FindAllCellsInRange : GridSearchStrategy
    {
        public override bool IsExitNode(GridSearchState state, Node node)
        {
            return false;
        }

        public override bool PassesCustomEnqueueConditions(GridSearchState state, Node node)
        {
            return node.StepsFromStart <= state.Character.TraversalRange;
        }

        public override int GetNodeCost(GridSearchState state, Node node)
        {
            return node.StepsFromStart;
        }
    }

    public class FindPathToCell : GridSearchStrategy
    {
        private Vector2Int _target;

        public FindPathToCell(Vector2Int target)
        {
            _target = target;
        }

        public override bool IsExitNode(GridSearchState state, Node node)
        {
            return node.Cell == _target;
        }

        public override bool PassesCustomEnqueueConditions(GridSearchState state, Node node)
        {
            return true;
        }

        public override int GetNodeCost(GridSearchState state, Node node)
        {
            return node.GetPathfindingCost(_target);
        }
    }

    public class FindPathToNearestEnemy : GridSearchStrategy
    {
        public override bool IsExitNode(GridSearchState state, Node node)
        {
            CellNeighbors neighbors = CellNeighbors.Get(node.Cell);
            for (int i = 0; i < CellNeighbors.Count; i++)
            {
                Vector2Int cell = neighbors[i];
                if (state.Character.IsEnemyInCell(cell, out Character enemy) && !enemy.IsDead)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool PassesCustomEnqueueConditions(GridSearchState state, Node node)
        {
            return true;
        }

        public override int GetNodeCost(GridSearchState state, Node node)
        {
            return node.StepsFromStart;
        }
    }
}