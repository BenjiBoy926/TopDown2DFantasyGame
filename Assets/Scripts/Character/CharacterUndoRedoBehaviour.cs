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
            gameObject.SetActive(true);
            yield return _character.FadeAlpha(1, _stepDuration, Ease.Linear);
            _character.PlayIdleAnimation();
        }

        if (_character.Health != state.Health)
        {
            _character.SetHealth(state.Health);
            yield return transform.DOPunchPosition(Vector3.up * .49f, _stepDuration, 0, 0).WaitForCompletion();
        }

        Vector2 position = _character.CellToWorld(state.Cell);
        yield return transform.DOMove(position, _stepDuration).WaitForCompletion();
        _character.SetDirection(state.Direction);
        _character.RefreshCell();
        _character.SetHasMovedThisTurn(state.HasMoved);

        if (isDead)
        {
            yield return _character.PlayDieAnimation();
            if (!_character.CanBeRevived)
            {
                yield return _character.FadeAlpha(0, _stepDuration, Ease.Linear);
                gameObject.SetActive(false);
            }
        }
        else
        {
            yield return _character.PerformSpriteFade(.1f);
        }
    }
}