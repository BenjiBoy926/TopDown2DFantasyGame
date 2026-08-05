using System;
using UnityEngine;

public abstract class GridSearchStrategy
{
    public Comparison<Node> NodeComparison => _nodeComparison ??= CompareNodes;

    private Comparison<Node> _nodeComparison;

    public abstract bool IsExitNode(GridSearchState state, Node node);
    public abstract bool PassesCustomEnqueueConditions(GridSearchState state, Node node);
    protected abstract int CompareNodes(Node a, Node b);

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

        protected override int CompareNodes(Node a, Node b)
        {
            return a.StepsFromStart.CompareTo(b.StepsFromStart);
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

        protected override int CompareNodes(Node a, Node b)
        {
            int aCost = a.GetPathfindingCost(_target);
            int bCost = b.GetPathfindingCost(_target);
            return aCost.CompareTo(bCost);
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

        protected override int CompareNodes(Node a, Node b)
        {
            return a.StepsFromStart.CompareTo(b.StepsFromStart);
        }
    }
}