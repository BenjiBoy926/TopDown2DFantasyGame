using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayerPathfinder : MonoBehaviour
{
    public class Node
    {
        public Vector2Int Cell;
        public Node Parent;
    }

    private List<Vector2Int> _path = new();

    public List<Vector2Int> FindPath(Character character, Vector2Int target)
    {
        _path.Clear();
        // add them
        return _path;
    }
}