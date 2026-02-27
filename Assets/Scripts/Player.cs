using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Grid))]
public class Player : MonoBehaviour, DefaultActions.IPlayerActions
{
    [SerializeField] private Transform _gridPosition;
    private Grid _grid;
    private Character _character;
    private DefaultActions _actions;
    private readonly Collider2D[] _overlapResults = new Collider2D[1];

    public void SetCharacter(Character character)
    {
        if (_character)
        {
            _character.Position = _grid.SnapToGrid(_character.Position);
            _character.SetIsRunning(false);
        }
        _character = character;
        if (_character)
        {
            _character.SetIsRunning(true);
        }
    }

    private void Awake()
    {
        _grid = GetComponent<Grid>();
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
        // Return unit back to original position
    }

    public void OnCursorPosition(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector2 oldPosition = transform.position;
        transform.position = newPosition;

        Vector2 positionOnGrid = _grid.SnapToGrid(newPosition);
        _gridPosition.position = positionOnGrid;

        if (_character)
        {
            _character.Position = newPosition;
            _character.SetDirection(newPosition - oldPosition);
        }
    }

    public void OnCursorPress(InputAction.CallbackContext context)
    {
        if (context.started && !_character)
        {
            CaptureNewCharacter();
        }
        if (context.canceled && _character)
        {
            SetCharacter(null);
        }
    }

    private void CaptureNewCharacter()
    {
        Character characterAtCursor = GetCharacterAtCursor();
        if (characterAtCursor)
        {
            SetCharacter(characterAtCursor);
        }
    }

    private Character GetCharacterAtCursor()
    {
        Vector2 gridPosition = _gridPosition.position;
        int count = Physics2D.OverlapBoxNonAlloc(gridPosition, Vector2.one, 0, _overlapResults);
        return count > 0 ? _overlapResults[0].GetComponentInParent<Character>() : null;
    }
}
