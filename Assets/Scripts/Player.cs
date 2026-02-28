using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, DefaultActions.IPlayerActions
{
    [SerializeField] private Transform _gridPosition;
    private Grid _grid;
    private Character _character;
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
        
    }

    public void OnAct(InputAction.CallbackContext context)
    {
        
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
        if (context.canceled && _character)
        {
            FinishMove();
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

        Vector2 positionOnGrid = _grid.Round(newPosition);
        _gridPosition.position = positionOnGrid;
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
            // TODO: change to coroutine that runs in character to smooth move
            _character.Position = _gridPosition.position;
            _character.SetIsRunning(false);
            SetCharacter(null);
        }
    }

    private void CancelMove()
    {
        if (_character)
        {
            // TODO: move character back to starting position
            // SetCharacter(null);
        }
    }

    private void SetCharacter(Character character)
    {
        _character = character;
        if (_character)
        {
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
