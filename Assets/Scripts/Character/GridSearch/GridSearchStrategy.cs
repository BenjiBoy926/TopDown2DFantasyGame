using System;
using UnityEngine;

public abstract class GridSearchStrategy
{
    public Comparison<Node> NodeComparison => _nodeComparison ??= CompareNodes;

    private Comparison<Node> _nodeComparison;

    public abstract bool PassesCustomEnqueueConditions(Node node);
    protected abstract int CompareNodes(Node a, Node b);
    public abstract bool IsExitNode(Node node);
    public abstract void OnExitNodeReached(Node node);

    public class FindAllCellsInRange : GridSearchStrategy
    {
        private readonly Character _character;

        public FindAllCellsInRange(Character character)
        {
            _character = character;
        }

        public override bool IsExitNode(Node node)
        {
            return false;
        }

        public override void OnExitNodeReached(Node node)
        {

        }

        public override bool PassesCustomEnqueueConditions(Node node)
        {
            return node.StepsFromStart <= _character.TraversalRange;
        }

        protected override int CompareNodes(Node a, Node b)
        {
            return a.StepsFromStart.CompareTo(b.StepsFromStart);
        }
    }

    public class FindPathToCell : GridSearchStrategy
    {
        public Node Result => _result;

        private Vector2Int _target;
        private Node _result;

        public FindPathToCell(Vector2Int target)
        {
            _target = target;
        }

        public override void OnExitNodeReached(Node node)
        {
            _result = node;
        }

        public override bool IsExitNode(Node node)
        {
            return node.Cell == _target;
        }

        public override bool PassesCustomEnqueueConditions(Node node)
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
}