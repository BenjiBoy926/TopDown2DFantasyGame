using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Grid))]
public class Player : MonoBehaviour, DefaultActions.IPlayerActions
{
    private Grid _grid;
    private Character _character;
    private DefaultActions _actions;
    private Collider2D[] _overlapResults = new Collider2D[1];

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
        if (_character)
        {
            Vector2 move = context.ReadValue<Vector2>();
            _character.SetDirection(move);
        }
    }

    public void OnAct(InputAction.CallbackContext context)
    {
        Character characterAtCursor = GetCharacterAtCursor();
        if (characterAtCursor && characterAtCursor != _character)
        {
            SetCharacter(characterAtCursor);
        }
        else if (_character)
        {
            _character.Attack(); 
        }
    }

    public void OnCursorPosition(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        transform.position = _grid.SnapWorldPositionToGrid(worldPosition);
    }

    public void OnCursorDelta(InputAction.CallbackContext context)
    {

    }

    private Character GetCharacterAtCursor()
    {
        Vector2 position = transform.position;
        int count = Physics2D.OverlapBoxNonAlloc(position, Vector2.one, 0, _overlapResults);
        return count > 0 ? _overlapResults[0].GetComponentInParent<Character>() : null;
    }
}
