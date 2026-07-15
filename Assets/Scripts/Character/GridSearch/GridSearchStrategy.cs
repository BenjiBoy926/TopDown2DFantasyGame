public abstract class GridSearchStrategy
{
    public abstract bool ShouldEnqueue(Node node);
    public abstract int CompareNodes(Node a, Node b);
    public abstract bool IsExitNode(Node node);
    public abstract void ExitNodeReached(Node node);
}