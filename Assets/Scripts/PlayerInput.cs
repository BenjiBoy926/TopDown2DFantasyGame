using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerCursor))]
public class PlayerInput : MonoBehaviour, DefaultActions.IPlayerActions
{
    private bool IsInputAllowed => !_cursor.IsTurnChangeAnimationPlaying;

    [SerializeField] private float _speed = 5;
    [SerializeField] private float _zoomChangeSpeed = 5;

    private PlayerCursor _cursor;
    private DefaultActions _actions;
    private Vector2 _moveDirection;
    private float _zoomDirection;

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
        float zoomChangeThisFrame = _zoomChangeSpeed * Time.deltaTime * _zoomDirection;
        _cursor.ChangeZoom(zoomChangeThisFrame);
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

    public void OnZoomMove(InputAction.CallbackContext context)
    {
        _zoomDirection = context.ReadValue<float>();
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

    public void OnZoomJump(InputAction.CallbackContext context)
    {
        float direction = context.ReadValue<float>();
        if (direction < 0)
        {
            _cursor.ZoomOut();
        }
        else if (direction > 0)
        {
            _cursor.ZoomIn();
        }
    }
}
