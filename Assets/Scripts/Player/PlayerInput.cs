using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour, DefaultActions.IPlayerActions
{
    private bool ShouldMoveFaster => (_isConfirmPressed && !_player.ActiveCharacter) || _isCancelPressed;

    [SerializeField] private float _speed = 5;
    [SerializeField] private float _fasterSpeed = 10;
    [SerializeField] private float _zoomChangeSpeed = 5;

    private Player _player;
    private DefaultActions _actions;
    private Vector2 _moveDirection;
    private bool _isConfirmPressed = false;
    private bool _isCancelPressed = false;
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
        if (!_player.IsInputAllowed)
            return;

        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            float speed = ShouldMoveFaster ? _fasterSpeed : _speed;
            Vector2 offsetThisFrame = speed * Time.deltaTime * _moveDirection;
            _player.SlidePosition(offsetThisFrame);
            _player.IncludeInView();
        }
        float zoomChangeThisFrame = _zoomChangeSpeed * Time.deltaTime * _zoomDirection;
        _player.ChangeZoom(zoomChangeThisFrame);

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Break();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
       _moveDirection = context.ReadValue<Vector2>();
    }

    public void OnCursorPosition(InputAction.CallbackContext context)
    {
        if (!_player.IsInputAllowed)
            return;

        Vector2 screenPosition = context.ReadValue<Vector2>();
        _player.SetScreenPosition(screenPosition);
        if (_player.ActiveCharacter)
        {
            _player.IncludeInView();
        }
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isConfirmPressed = true;
        }
        else if (context.canceled)
        {
            _isConfirmPressed = false;
        }

        if (!_player.IsInputAllowed)
            return;

        if (context.started)
        {
            _player.Grab();
        }
        else if (context.canceled)
        {
            _player.Release();
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        _player.CancelMove();
        if (context.started)
        {
            _isCancelPressed = true;
        }
        if (context.canceled)
        {
            _isCancelPressed = false;
        }
    }

    public void OnZoomMove(InputAction.CallbackContext context)
    {
        _zoomDirection = context.ReadValue<float>();
    }

    public void OnZoomJump(InputAction.CallbackContext context)
    {
        if (!_player.IsInputAllowed)
            return;

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
        if (!_player.IsInputAllowed)
            return;

        if (context.started)
        {
            _player.StartUndo();
        }
        else if (context.canceled)
        {
            _player.StopUndo();
        }
    }

    public void OnRedo(InputAction.CallbackContext context)
    {
        if (!_player.IsInputAllowed)
            return;

        if (context.started)
        {
            _player.StartRedo();
        }
        else if (context.canceled)
        {
            _player.StopRedo();
        }
    }
}
