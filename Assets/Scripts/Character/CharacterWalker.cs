using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterWalker : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    private readonly List<Vector2Int> _path = new();
    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public Coroutine WalkToNodeClamped(Node node)
    {
        BuildPathClamped(node, _path);
        StopAllCoroutines();
        return StartCoroutine(WalkOnPath(_path));
    }

    private IEnumerator WalkOnPath(List<Vector2Int> path)
    {
        _character.SetIsRunning(true);
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int nextCell = path[i];
            yield return WalkDirectlyToCell(nextCell);
        }
        _character.SetIsRunning(false);
    }

    private YieldInstruction WalkDirectlyToCell(Vector2Int cell)
    {
        Vector2 nextPosition = _character.CellToWorld(cell);
        _character.LookAt(nextPosition);
        return _character.transform.DOMove(nextPosition, _speed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }

    private void BuildPathClamped(Node node, List<Vector2Int> path)
    {
        path.Clear();
        while (node != null)
        {
            if (node.StepsFromStart <= _character.TraversalRange)
            {
                path.Add(node.Cell);
            }
            node = node.Parent;
        }
        path.Reverse();
    }
}