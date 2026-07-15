using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayerPathfinder : MonoBehaviour
{
    [SerializeField] private float _speed = 2;
    private readonly List<Vector2Int> _path = new();

    public IEnumerator MoveToCell(Character character, Vector2Int cell)
    {
        character.FindPath(cell, _path);

        character.SetIsRunning(true);
        for (int i = 0; i < _path.Count; i++)
        {
            Vector2Int nextCell = _path[i];
            yield return MoveDirectlyToCell(character, nextCell);
        }
        character.SetIsRunning(false);
    }

    private YieldInstruction MoveDirectlyToCell(Character character, Vector2Int cell)
    {
        Vector2 nextPosition = character.CellToWorld(cell);
        character.LookAt(nextPosition);
        return character.transform.DOMove(nextPosition, _speed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }
}