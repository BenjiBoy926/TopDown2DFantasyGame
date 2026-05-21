using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Character))]
public class CharacterUndoRedoBehaviour : MonoBehaviour
{
    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void ApplyState(CharacterState state)
    {
        // TODO: refactor to store vector2 and not two separate enums
        _character.SetDirection(state.HorizontalDirection, state.VerticalDirection);
        _character.SetHealth(state.Health);

        bool isActive = !_character.IsDead || _character.CanBeRevived;
        gameObject.SetActive(isActive);
        if (_character.IsDead && isActive)
        {
            _character.PlayDieAnimation();
        }
        if (!_character.IsDead)
        {
            _character.PlayIdleAnimation();
        }

        _character.Position = _character.CellToWorld(state.Cell);
        if (isActive)
        {
            _character.RefreshCell();
        }

        _character.SetHasMovedThisTurn(state.HasMoved);
        _character.UpdateSpriteFadeColorImmediately();
    }
}