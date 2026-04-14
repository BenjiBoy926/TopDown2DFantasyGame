using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsTurnChangeAnimationPlaying => _battle.IsTurnChangeAnimationPlaying;
    public bool IsCameraGrabbed => _camera.IsGrabbed;
    public Character ActiveCharacter => _activeCharacter;

    [SerializeField] private Transform _exactPosition;
    [SerializeField] private Transform _gridPosition;
    [SerializeField] private PlayerCamera _camera;

    private Battle _battle;
    private Character _activeCharacter;
    private Character _hoveredCharacter;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
    }

    public void IncludeInView()
    {
        _camera.IncludeInView(_exactPosition.position);
    }

    public void ChangeZoom(float zoom)
    {
        _camera.ChangeZoom(zoom);
    }

    public void ZoomIn()
    {
        _camera.ZoomIn();
    }

    public void ZoomOut()
    {
        _camera.ZoomOut();
    }

    public void SlidePosition(Vector2 offset)
    {
        Vector2 position = _exactPosition.position;
        SetPosition(position + offset);
    }

    public void SetScreenPosition(Vector2 screenPosition)
    {
        if (IsCameraGrabbed)
        {
            _camera.GrabUpdate(screenPosition);
        }
        else
        {
            Vector2 worldPosition = _camera.ScreenToWorld(screenPosition);
            SetPosition(worldPosition);
        }
    }

    public void SetPosition(Vector2 newPosition)
    {
        if (_activeCharacter)
        {
            Vector2 oldPosition = _activeCharacter.Position;
            _activeCharacter.Position = _activeCharacter.ClampToTraversibleCells(newPosition);
            _activeCharacter.SetDirection(newPosition - oldPosition);

            _exactPosition.position = _activeCharacter.ClampToReachableCells(newPosition);
            _gridPosition.position = _battle.CellToWorld(_activeCharacter.CurrentCell);
        }
        else
        {
            _exactPosition.position = newPosition;
            _gridPosition.position = _battle.SnapToGrid(newPosition);
        }
        UpdateHoveredCharacter();
    }

    public void Grab()
    {
        Character characterAtCursor = GetCharacterAtCursor();
        if (characterAtCursor)
        {
            StartMove();
        }
        else
        {
            _camera.Grab(_exactPosition.position);
        }
    }

    public void Release()
    {
        if (_activeCharacter)
        {
            FinishMove();
        }
        else
        {
            _camera.Release();
        }
    }

    public void StartMove()
    {
        Character characterAtCursor = GetCharacterAtCursor();
        if (CanMoveCharacter(characterAtCursor))
        {
            SetActiveCharacter(characterAtCursor);
        }
    }

    public void FinishMove()
    {
        if (!_activeCharacter) return;

        if (_activeCharacter.CanStayInCell(_activeCharacter.CurrentCell))
        {
            ConfirmMove();
        }
        else
        {
            CancelMove();
        }
    }

    private void ConfirmMove()
    {
        if (!_activeCharacter)
            return;

        Vector2Int targettingCell = _battle.WorldToCell(_exactPosition.position);
        Character target = _battle.GetOccupant(targettingCell);
        if (target && target.Faction != _activeCharacter.Faction)
        {
            _activeCharacter.Attack(target);
        }
        else
        {
            _activeCharacter.Defend();
        }

        Deselect();
    }


    public void CancelMove()
    {
        if (_activeCharacter)
        {
            _activeCharacter.Cancel();
            Deselect();
        }
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

    private void Deselect()
    {
        SetHoveredCharacter(null);
        SetActiveCharacter(null);
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

    private void SetActiveCharacter(Character character)
    {
        _activeCharacter = character;
        if (_activeCharacter)
        {
            _activeCharacter.Position = _exactPosition.position;
            _activeCharacter.SetIsRunning(true);
        }
    }

    private Character GetCharacterAtCursor()
    {
        Vector2Int cell = _battle.WorldToCell(_gridPosition.position);
        return _battle.GetOccupant(cell);
    }

    private bool CanMoveCharacter(Character character)
    {
        return character && !character.HasMovedThisTurn && character.Faction == _battle.CurrentFactionTurn;
    }
}
