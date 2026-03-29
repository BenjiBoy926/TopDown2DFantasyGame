using DG.Tweening;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public bool IsTurnChangeAnimationPlaying => _battle.IsTurnChangeAnimationPlaying;
    public Character ActiveCharacter => _activeCharacter;

    [SerializeField] private Transform _gridPosition;

    private Battle _battle;
    private Character _activeCharacter;
    private Character _hoveredCharacter;
    private Vector2 _capturePosition;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
    }

    public void SlidePosition(Vector2 offset)
    {
        Vector2 position = transform.position;
        SetPosition(position + offset);
    }

    public void SetPosition(Vector2 newPosition)
    {
        if (_activeCharacter)
        {
            Vector2 oldPosition = _activeCharacter.Position;
            _activeCharacter.Position = _activeCharacter.ClampToTraversibleCells(newPosition);
            _activeCharacter.SetDirection(newPosition - oldPosition);

            Vector2Int closestCell = _activeCharacter.ClosestTraversibleCell(newPosition);
            transform.position = _activeCharacter.ClampToReachableCells(newPosition);
            _gridPosition.position = _battle.CellToWorld(closestCell);
        }
        else
        {
            transform.position = newPosition;
            _gridPosition.position = _battle.SnapToGrid(newPosition);
        }
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

    public void StartMove()
    {
        Character characterAtCursor = GetCharacterAtCursor();
        if (characterAtCursor && !characterAtCursor.HasMovedThisTurn)
        {
            SetCharacter(characterAtCursor);
        }
    }

    public void FinishMove()
    {
        if (!_activeCharacter) return;

        Vector2Int intendedCell = _battle.WorldToCell(_gridPosition.position);
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
            Vector2Int cell = _battle.WorldToCell(_gridPosition.position);
            _activeCharacter.Wait(cell);
            SetHoveredCharacter(null);
            SetCharacter(null);
        }
    }

    public void CancelMove()
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
        Vector2Int cell = _battle.WorldToCell(_gridPosition.position);
        return _battle.GetOccupant(cell);
    }
}
