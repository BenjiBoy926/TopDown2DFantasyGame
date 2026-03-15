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
    private Character _activeCharacter;
    private Character _hoveredCharacter;
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
        if (!_activeCharacter)
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
        if (context.started && !_activeCharacter)
        {
            StartMove();
        }
        else if (context.canceled && _activeCharacter)
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
        if (_activeCharacter)
        {
            _activeCharacter.Position = newPosition;
            _activeCharacter.SetDirection(offset);
        }
        UpdateHoveredCharacter();
    }

    private void SetPosition(Vector2 newPosition)
    {
        if (_activeCharacter)
        {
            Vector2 oldPosition = transform.position;
            _activeCharacter.Position = newPosition;
            _activeCharacter.SetDirection(newPosition - oldPosition);
        }
        transform.position = newPosition;
        _gridPosition.position = _battle.SnapToGrid(newPosition);
        UpdateHoveredCharacter();
    }

    private void UpdateHoveredCharacter()
    {
        if (_activeCharacter)
        {
            SetHoveredCharacter(_activeCharacter);
        }
        else
        {
            SetHoveredCharacter(GetCharacterAtCursor());
        }
    }

    private void SetHoveredCharacter(Character hoveredCharacter)
    {
        if (hoveredCharacter == _hoveredCharacter) return;
    
        if (_hoveredCharacter)
        {
            _hoveredCharacter.HideRange();
        }
        _hoveredCharacter = hoveredCharacter;
        if (_hoveredCharacter)
        {
            _hoveredCharacter.ShowRange();
        }
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
        if (!_activeCharacter) return;

        Vector3Int intendedCell = _battle.WorldToCellRounded(_activeCharacter.Position);
        Character occupant = _battle.GetOccupant(intendedCell);
        if (occupant && occupant != _activeCharacter)
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
        if (_activeCharacter)
        {
            _activeCharacter.Wait();
            SetHoveredCharacter(null);
            SetCharacter(null);
        }
    }

    private void CancelMove()
    {
        if (_activeCharacter)
        {
            _activeCharacter.RunTo(_capturePosition, Ease.OutBack, 0.35f);
            SetHoveredCharacter(null);
            SetCharacter(null);
        }
    }

    private void SetCharacter(Character character)
    {
        _activeCharacter = character;
        if (_activeCharacter)
        {
            _capturePosition = _activeCharacter.Position;
            _activeCharacter.Position = transform.position;
            _activeCharacter.SetIsRunning(true);
        }
    }

    private Character GetCharacterAtCursor()
    {
        Vector3Int cell = _battle.WorldToCellRounded(_gridPosition.position);
        return _battle.GetOccupant(cell);
    }
}
