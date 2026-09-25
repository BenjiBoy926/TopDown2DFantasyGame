using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class GridSearchStrategy
{
    public abstract void Start(GridSearchState state);
    public abstract Node GetNeighbor(GridSearchState state, NodeNeighbors neighbors, int i);
    public abstract bool IsExitNode(GridSearchState state, Node node);
    public abstract bool PassesCustomEnqueueConditions(GridSearchState state, Node node);
    public abstract int GetNodeCost(GridSearchState state, Node node);

    protected void BuildNeighborIndexTable(Vector2Int direction, int[] table)
    {
        const int first = 0;
        const int second = 1;
        const int third = 2;
        const int fourth = 3;

        const int up = 0;
        const int right = 1;
        const int down = 2;
        const int left = 3;

        bool isRight = direction.x > 0;
        bool isUp = direction.y > 0;
        bool isHorizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
        
        if (isHorizontal)
        {
            table[first] = isRight ? right : left;
            table[second] = isUp ? up : down;
            table[third] = isRight ? left : right;
            table[fourth] = isUp ? down : up;
        }
        else
        {
            table[first] = isUp ? up : down;
            table[second] = isRight ? right : left;
            table[third] = isUp ? down : up;
            table[fourth] = isRight ? left : right;
        }
    }

    public sealed class FindAllCellsInRange : GridSearchStrategy
    {
        public override void Start(GridSearchState state)
        {
            // do nothing
        }

        public override Node GetNeighbor(GridSearchState state, NodeNeighbors neighbors, int i)
        {
            return neighbors[i];
        }

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

    public sealed class FindPathToCell : GridSearchStrategy
    {
        private Vector2Int _target;
        private readonly int[] _neighborIndexTable = new int[4];

        public FindPathToCell(Vector2Int target)
        {
            _target = target;
        }

        public override void Start(GridSearchState state)
        {
            Vector2Int targetOffset = _target - state.Character.CurrentCell;
            BuildNeighborIndexTable(targetOffset, _neighborIndexTable);
        }

        public override Node GetNeighbor(GridSearchState state, NodeNeighbors neighbors, int i)
        {
            int index = _neighborIndexTable[i];
            return neighbors[index];
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

    public sealed class FindPathToNearestEnemy : GridSearchStrategy
    {
        private readonly int[] _neighborIndexTable = new int[4];

        public override void Start(GridSearchState state)
        {
            Vector2Int averageEnemyOffset = CalculateAverageEnemyOffset(state);
            Vector2Int targetOffset = averageEnemyOffset - state.Character.CurrentCell;
            BuildNeighborIndexTable(targetOffset, _neighborIndexTable);
        }

        private static Vector2Int CalculateAverageEnemyOffset(GridSearchState state)
        {
            Vector2Int averageEnemyOffset = Vector2Int.zero;
            int enemyCount = 0;
            foreach (var character in state.Character.AllCharactersInBattle)
            {
                if (state.Character.Faction != character.Faction)
                {
                    averageEnemyOffset += character.CurrentCell;
                    enemyCount++;
                }
            }
            averageEnemyOffset /= enemyCount;
            return averageEnemyOffset;
        }

        public override Node GetNeighbor(GridSearchState state, NodeNeighbors neighbors, int i)
        {
            int index = _neighborIndexTable[i];
            return neighbors[index];
        }

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