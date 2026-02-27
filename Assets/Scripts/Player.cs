using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Grid))]
public class Player : MonoBehaviour, DefaultActions.IPlayerActions
{
    private Grid _grid;
    private Character _character;
    private DefaultActions _actions;
    private readonly Collider2D[] _overlapResults = new Collider2D[1];

    public void SetCharacter(Character character)
    {
        if (_character)
        {
            _character.Position = _grid.SnapWorldPositionToGrid(_character.Position);
        }
        _character = character;
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

        if (_character)
        {
            _character.Position = newPosition;
            _character.SetDirection(newPosition - oldPosition);
            _character.RunForTime(.5f);
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
        Vector2 position = transform.position;
        Vector2 gridPosition = _grid.SnapWorldPositionToGrid(position);
        int count = Physics2D.OverlapBoxNonAlloc(gridPosition, Vector2.one, 0, _overlapResults);
        return count > 0 ? _overlapResults[0].GetComponentInParent<Character>() : null;
    }
}
