using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Character))]
public class CharacterDefendBehaviour : MonoBehaviour
{
    [SerializeField] private float _moveDuration = .35f;
    [SerializeField] private Ease _moveEase = Ease.OutCubic;
    private Character _character;

    public IEnumerator GetSequence()
    {
        _character.SecureCurrentCell();

        Vector2 targetPosition = _character.CellToWorld(_character.CurrentCell);
        _character.LookAt(targetPosition);

        _character.SetIsRunning(true);
        yield return transform.DOMove(targetPosition, _moveDuration)
            .SetEase(_moveEase)
            .WaitForCompletion();
        _character.SetIsRunning(false);

        yield return _character.MoveFadeOut();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}