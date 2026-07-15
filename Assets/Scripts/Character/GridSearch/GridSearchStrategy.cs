using System;

public abstract class GridSearchStrategy
{
    public Comparison<Node> NodeComparison => _nodeComparison ??= CompareNodes;

    private Comparison<Node> _nodeComparison;

    public abstract bool ShouldEnqueue(Node node);
    public abstract int CompareNodes(Node a, Node b);
    public abstract bool IsExitNode(Node node);
    public abstract void ExitNodeReached(Node node);
}