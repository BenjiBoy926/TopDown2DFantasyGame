using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Grid))]
public class Player : MonoBehaviour, DefaultActions.ICharacterActions
{
    private Grid _grid;
    private Character _character;
    private DefaultActions _actions;

    public void SetCharacter(Character character)
    {
        if (_character)
        {
            _character.Position = _grid.SnapWorldPositionToGrid(_character.Position);
        }
        _character = character;
    }

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _actions = new();
        _actions.Character.AddCallbacks(this);
    }

    private void OnEnable()
    {
        _actions.Enable();   
    }

    private void OnDisable()
    {
        _actions.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (_character)
        {
            Vector2 move = context.ReadValue<Vector2>();
            _character.SetDirection(move);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (_character)
        {
            _character.Attack();
        }
    }
}
