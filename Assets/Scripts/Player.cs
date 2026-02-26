using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, DefaultActions.ICharacterActions
{
    private Character _character;
    private DefaultActions _actions;

    public void SetCharacter(Character character)
    {
        _character = character;
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
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
