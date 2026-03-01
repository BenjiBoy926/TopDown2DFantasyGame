using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player : MonoBehaviour, DefaultActions.IPlayerActions
{
    [SerializeField] private Transform _gridPosition;
    [SerializeField] private float _speed = 5;

    private Battlefield _battlefield;
    private DefaultActions _actions;
    private readonly Collider2D[] _overlapResults = new Collider2D[1];

    private Vector2 _moveDirection;
    private Character _character;
    private Vector2 _capturePosition;

    private void Awake()
    {
        _battlefield = GetComponentInParent<Battlefield>();
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
        ReturnCharacter();
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
        _gridPosition.position = _battlefield.SnapToGrid(newPosition);
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
        _gridPosition.position = _battlefield.SnapToGrid(newPosition);
    }

    private void StartMove()
    {
        Character characterAtCursor = GetCharacterAtCursor();
        if (characterAtCursor)
        {
            SetCharacter(characterAtCursor);
        }
    }

    private void FinishMove()
    {
        if (!_character) return;

        Vector3Int intendedCell = _battlefield.WorldToCellRounded(_character.Position);
        if (_battlefield.GetOccupant(intendedCell))
        {
            ReturnCharacter();
        }
        else
        {
            PlaceCharacter();
        }
    }

    private void PlaceCharacter()
    {
        if (_character)
        {
            _character.RunTo(_gridPosition.position, Ease.OutCirc, 0.35f);
            _battlefield.RefreshOccupantCell(_character);
            SetCharacter(null);
        }
    }

    private void ReturnCharacter()
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
        Vector3Int cell = _battlefield.WorldToCellRounded(_gridPosition.position);
        return _battlefield.GetOccupant(cell);
    }
}
