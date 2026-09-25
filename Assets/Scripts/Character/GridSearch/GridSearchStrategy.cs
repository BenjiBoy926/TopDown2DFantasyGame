using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class GridSearchStrategy
{
    public abstract void Start(GridSearchState state);
    public abstract Node GetNeighbor(GridSearchState state, NodeNeighbors neighbors, int i);
    public abstract bool IsExitNode(GridSearchState state, Node node);
    public abstract bool PassesCustomEnqueueConditions(GridSearchState state, Node node);
    public abstract int GetNodeCost(GridSearchState state, Node node);

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
            Vector2Int absoluteOffset = new(Mathf.Abs(targetOffset.x), Mathf.Abs(targetOffset.y));

            int first = 0;
            int second = 1;
            int third = 2;
            int fourth = 3;

            int up = 0;
            int right = 1;
            int down = 2;
            int left = 3;

            if (absoluteOffset.x > absoluteOffset.y)
            {
                if (targetOffset.x > 0)
                {
                    _neighborIndexTable[first] = right;
                    _neighborIndexTable[third] = left;
                    if (targetOffset.y > 0)
                    {
                        _neighborIndexTable[second] = up;
                        _neighborIndexTable[fourth] = down;
                    }
                    else
                    {
                        _neighborIndexTable[second] = down;
                        _neighborIndexTable[fourth] = up;
                    }
                }
                else
                {
                    _neighborIndexTable[first] = left;
                    _neighborIndexTable[third] = right;
                    if (targetOffset.x > 0)
                    {
                        _neighborIndexTable[second] = right;
                        _neighborIndexTable[fourth] = left;
                    }
                    else
                    {
                        _neighborIndexTable[second] = left;
                        _neighborIndexTable[fourth] = right;
                    }
                }
            }
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

            Vector2Int targetOffset = averageEnemyOffset - state.Character.CurrentCell;
            Vector2Int absoluteOffset = new(Mathf.Abs(targetOffset.x), Mathf.Abs(targetOffset.y));

            int first = 0;
            int second = 1;
            int third = 2;
            int fourth = 3;

            int up = 0;
            int right = 1;
            int down = 2;
            int left = 3;

            if (absoluteOffset.x > absoluteOffset.y)
            {
                if (targetOffset.x > 0)
                {
                    _neighborIndexTable[first] = right;
                    _neighborIndexTable[third] = left;
                    if (targetOffset.y > 0)
                    {
                        _neighborIndexTable[second] = up;
                        _neighborIndexTable[fourth] = down;
                    }
                    else
                    {
                        _neighborIndexTable[second] = down;
                        _neighborIndexTable[fourth] = up;
                    }
                }
                else
                {
                    _neighborIndexTable[first] = left;
                    _neighborIndexTable[third] = right;
                    if (targetOffset.x > 0)
                    {
                        _neighborIndexTable[second] = right;
                        _neighborIndexTable[fourth] = left;
                    }
                    else
                    {
                        _neighborIndexTable[second] = left;
                        _neighborIndexTable[fourth] = right;
                    }
                }
            }
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