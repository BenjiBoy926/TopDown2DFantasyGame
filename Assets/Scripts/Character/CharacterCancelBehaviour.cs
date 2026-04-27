using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Character))]
public class CharacterCancelBehaviour : MonoBehaviour
{
    [SerializeField] private float _moveDuration = .35f;
    [SerializeField] private Ease _moveEase = Ease.OutBack;
    private Character _character;

    public IEnumerator GetSequence()
    {
        Vector2 targetPosition = _character.CellToWorld(_character.HomeCell);
        _character.LookAt(targetPosition);
        _character.SetIsRunning(true);
        yield return transform.DOMove(targetPosition, _moveDuration)
            .SetEase(_moveEase)
            .WaitForCompletion();
        _character.SetIsRunning(false);
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}