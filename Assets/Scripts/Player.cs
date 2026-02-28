using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player : MonoBehaviour, DefaultActions.IPlayerActions
{
    [SerializeField] private Transform _gridPosition;
    [SerializeField] private float _speed = 5;
    private Grid _grid;
    private Character _character;
    private Vector2 _capturePosition;
    private DefaultActions _actions;
    private readonly Collider2D[] _overlapResults = new Collider2D[1];

    private void Awake()
    {
        _grid = GetComponentInParent<Grid>();
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

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        SlidePosition(direction);
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

    private void SlidePosition(Vector2 direction)
    {
        if (_character)
        {
            _character.SetDirection(direction);
        }
        transform.Translate(_speed * Time.deltaTime * direction);
        _gridPosition.position = _grid.Round(transform.position);
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
        _gridPosition.position = _grid.Round(newPosition);
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
        if (_character)
        {
            _character.RunTo(_gridPosition.position, Ease.OutCirc, 0.35f);
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
        Vector2 gridPosition = _gridPosition.position;
        int count = Physics2D.OverlapBoxNonAlloc(gridPosition, Vector2.one, 0, _overlapResults);
        return count > 0 ? _overlapResults[0].GetComponentInParent<Character>() : null;
    }
}
