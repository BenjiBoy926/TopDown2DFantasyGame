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
            _cursor.IncludeInView();
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
        _cursor.SetScreenPosition(screenPosition);
        if (_cursor.ActiveCharacter)
        {
            _cursor.IncludeInView();
        }
    }

    public void OnCursorPress(InputAction.CallbackContext context)
    {
        if (!IsInputAllowed) return;
        if (context.started)
        {
            _cursor.Grab();
        }
        else if (context.canceled)
        {
            _cursor.Release();
        }
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        float direction = context.ReadValue<float>();
        if (direction < 0)
        {
            _cursor.IncreaseViewSize();
        }
        else if (direction > 0)
        {
            _cursor.DecreaseViewSize();
        }
    }
}
