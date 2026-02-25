using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
public class Player : MonoBehaviour, DefaultActions.ICharacterActions
{
    private Character _character;
    private DefaultActions _actions;

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
        Vector2 move = context.ReadValue<Vector2>();
        _character.SetDirection(move);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        _character.Attack();
    }
}
