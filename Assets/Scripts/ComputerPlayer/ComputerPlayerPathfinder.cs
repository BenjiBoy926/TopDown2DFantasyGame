using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayerPathfinder : MonoBehaviour
{
    [SerializeField] private float _speed = 2;

    public IEnumerator MoveToCell(Character character, Vector2Int cell)
    {
        List<Vector2Int> path = character.FindPath(cell);
        character.SetIsRunning(true);
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int nextCell = path[i];
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