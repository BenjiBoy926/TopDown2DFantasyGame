using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterUndoRedoBehaviour : MonoBehaviour
{
    [SerializeField] private float _stepDuration = .2f;
    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public IEnumerator GetApplyStateSequence(CharacterState state)
    {
        bool wasDead = _character.IsDead;
        bool isDead = state.Health <= 0;
        if (wasDead && !isDead)
        {
            yield return ShowRevival();
        }

        _character.SetHealth(state.Health);
        if (_character.CurrentCell != state.Cell)
        {
            yield return ShowCellChange(state);
        }

        _character.SetDirection(state.Direction);
        _character.SetHasMovedThisTurn(state.HasMoved);

        if (isDead)
        {
            yield return ShowDeath();
        }
        else
        {
            yield return _character.PerformSpriteFade(.1f);
        }
    }

    private IEnumerator ShowRevival()
    {
        gameObject.SetActive(true);
        yield return _character.FadeAlpha(1, _stepDuration, Ease.Linear);
        _character.PlayIdleAnimation();
        yield return transform.DOPunchPosition(Vector3.up * .49f, _stepDuration, 0, 0).WaitForCompletion();
    }

    private IEnumerator ShowCellChange(CharacterState state)
    {
        Vector2 position = _character.CellToWorld(state.Cell);
        yield return transform.DOMove(position, _stepDuration).WaitForCompletion();
    }

    private IEnumerator ShowDeath()
    {
        yield return _character.PlayDieAnimation();
        if (!_character.CanBeRevived)
        {
            yield return _character.FadeAlpha(0, _stepDuration, Ease.Linear);
            gameObject.SetActive(false);
        }
    }
}