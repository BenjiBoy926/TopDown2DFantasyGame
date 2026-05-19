using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour, DefaultActions.IPlayerActions
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _zoomChangeSpeed = 5;

    private Player _player;
    private DefaultActions _actions;
    private Vector2 _moveDirection;
    private float _zoomDirection;

    private void Awake()
    {
        _player = GetComponent<Player>();
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
            _player.SlidePosition(offsetThisFrame);
            _player.IncludeInView();
        }
        float zoomChangeThisFrame = _zoomChangeSpeed * Time.deltaTime * _zoomDirection;
        _player.ChangeZoom(zoomChangeThisFrame);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
       _moveDirection = context.ReadValue<Vector2>();
    }

    public void OnAct(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!_player.ActiveCharacter)
        {
            _player.StartMove();
        }
        else
        {
            _player.FinishMove();
        }
    }

    public void OnZoomMove(InputAction.CallbackContext context)
    {
        _zoomDirection = context.ReadValue<float>();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        _player.CancelMove();
    }

    public void OnCursorPosition(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        _player.SetScreenPosition(screenPosition);
        if (_player.ActiveCharacter)
        {
            _player.IncludeInView();
        }
    }

    public void OnCursorPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _player.Grab();
        }
        else if (context.canceled)
        {
            _player.Release();
        }
    }

    public void OnZoomJump(InputAction.CallbackContext context)
    {
        float direction = context.ReadValue<float>();
        if (direction < 0)
        {
            _player.ZoomOut();
        }
        else if (direction > 0)
        {
            _player.ZoomIn();
        }
    }

    public void OnUndo(InputAction.CallbackContext context)
    {
        _player.Undo();
    }

    public void OnRedo(InputAction.CallbackContext context)
    {
        _player.Redo();
    }
}
