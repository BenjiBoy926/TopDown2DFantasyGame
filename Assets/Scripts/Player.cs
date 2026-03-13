using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player : MonoBehaviour, DefaultActions.IPlayerActions
{
    private bool IsInputAllowed => !_battle.IsTurnChangeAnimationPlaying;

    [SerializeField] private Transform _gridPosition;
    [SerializeField] private float _speed = 5;

    private Battle _battle;
    private DefaultActions _actions;

    private Vector2 _moveDirection;
    private Character _character;
    private Vector2 _capturePosition;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
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
            SlidePosition(offsetThisFrame);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
       _moveDirection = context.ReadValue<Vector2>();
    }

    public void OnAct(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!_character)
        {
            StartMove();
        }
        else
        {
            FinishMove();
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        CancelMove();
    }

    public void OnCursorPosition(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        SetPosition(newPosition);
    }

    public void OnCursorPress(InputAction.CallbackContext context)
    {
        if (context.started && !_character)
        {
            StartMove();
        }
        else if (context.canceled && _character)
        {
            FinishMove();
        }
    }

    // NOTE: slight repetition between SlidePosition and SetPosition
    // to avoid expensive calls to transform.position
    private void SlidePosition(Vector2 offset)
    {
        transform.Translate(offset);

        Vector2 newPosition = transform.position;
        _gridPosition.position = _battle.SnapToGrid(newPosition);
        if (_character)
        {
            _character.Position = newPosition;
            _character.SetDirection(offset);
        }
    }

    private void SetPosition(Vector2 newPosition)
    {
        if (_character)
        {
            Vector2 oldPosition = transform.position;
            _character.Position = newPosition;
            _character.SetDirection(newPosition - oldPosition);
        }
        transform.position = newPosition;
        _gridPosition.position = _battle.SnapToGrid(newPosition);
    }

    private void StartMove()
    {
        if (!IsInputAllowed) return;

        Character characterAtCursor = GetCharacterAtCursor();
        if (characterAtCursor && !characterAtCursor.HasMovedThisTurn)
        {
            SetCharacter(characterAtCursor);
        }
    }

    private void FinishMove()
    {
        if (!_character) return;

        Vector3Int intendedCell = _battle.WorldToCellRounded(_character.Position);
        Character occupant = _battle.GetOccupant(intendedCell);
        if (occupant && occupant != _character)
        {
            CancelMove();
        }
        else
        {
            ConfirmMove();
        }
    }

    private void ConfirmMove()
    {
        if (_character)
        {
            _character.Wait();
            SetCharacter(null);
        }
    }

    private void CancelMove()
    {
        if (_character)
        {
            _character.RunTo(_capturePosition, Ease.OutBack, 0.35f);
            SetCharacter(null);
        }
    }

    private void SetCharacter(Character character)
    {
        _character = character;
        if (_character)
        {
            _capturePosition = _character.Position;
            _character.Position = transform.position;
            _character.SetIsRunning(true);
        }
    }

    private Character GetCharacterAtCursor()
    {
        Vector3Int cell = _battle.WorldToCellRounded(_gridPosition.position);
        return _battle.GetOccupant(cell);
    }
}
