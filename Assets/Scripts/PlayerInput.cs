using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerCursor))]
public class PlayerInput : MonoBehaviour, DefaultActions.IPlayerActions
{
    private bool IsInputAllowed => !_cursor.IsTurnChangeAnimationPlaying;

    [SerializeField] private float _speed = 5;

    private PlayerCursor _cursor;
    private DefaultActions _actions;
    private Vector2 _moveDirection;

    private void Awake()
    {
        _cursor = GetComponent<PlayerCursor>();
        _actions = new();
        _actions.Player.AddCallbacks(this);
    }

    private void OnEnable()
    {
        _actions.Enable();   
    }

    private void OnDisable()
    {
        _actions.Disable();
    }

    private void Update()
    {
        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            Vector2 offsetThisFrame = _speed * Time.deltaTime * _moveDirection;
            _cursor.SlidePosition(offsetThisFrame);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
       _moveDirection = context.ReadValue<Vector2>();
    }

    public void OnAct(InputAction.CallbackContext context)
    {
        if (!IsInputAllowed) return;
        if (!context.started) return;
        if (!_cursor.ActiveCharacter)
        {
            _cursor.StartMove();
        }
        else
        {
            _cursor.FinishMove();
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        _cursor.CancelMove();
    }

    public void OnCursorPosition(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        _cursor.SetPosition(newPosition);
    }

    public void OnCursorPress(InputAction.CallbackContext context)
    {
        if (!IsInputAllowed) return;
        if (context.started && !_cursor.ActiveCharacter)
        {
            _cursor.StartMove();
        }
        else if (context.canceled && _cursor.ActiveCharacter)
        {
            _cursor.FinishMove();
        }
    }
}
