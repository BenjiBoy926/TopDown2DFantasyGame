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
        _character.SetDirection(state.Direction);
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