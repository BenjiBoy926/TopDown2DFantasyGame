using Hellmade.Sound;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsCameraGrabbed => _camera.IsGrabbed;
    public Character ActiveCharacter => _activeCharacter;

    [SerializeField] private AudioSource _cellHoverAudio;
    [SerializeField] private AudioClip _moveStartClip;

    private Battle _battle;
    private PlayerCamera _camera;
    private PlayerCursor _cursor;
    private PlayerGridReticle _gridReticle;
    private Character _activeCharacter;
    private Character _hoveredCharacter;
    private Vector2Int _currentCell;
    private bool _isInputAllowed = true;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _camera = GetComponentInChildren<PlayerCamera>();
        _cursor = GetComponentInChildren<PlayerCursor>();
        _gridReticle = GetComponentInChildren<PlayerGridReticle>();
    }

    private void Update()
    {
        RefreshIsInputAllowed();
    }

    public void StartNextTurn()
    {
        if (_isInputAllowed)
        {
            _battle.StartNextTurn();
        }
    }

    public void Undo()
    {
        if (_isInputAllowed)
        {
            _battle.Undo();
        }
    }

    public void Redo()
    {
        if (_isInputAllowed)
        {
            _battle.Redo();
        }
    }

    public void IncludeInView()
    {
        _camera.IncludeInView(_cursor.Position);
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
        Vector2 Position = _cursor.Position;
        SetPosition(Position + offset);
    }

    public void SetScreenPosition(Vector2 screenPosition)
    {
        if (!_isInputAllowed) return;

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
        if (!_isInputAllowed) return;

        SetActiveCharacterPosition(newPosition);
        SetCursorPosition(newPosition);
        RefreshCurrentCell();
    }

    public void Grab()
    {
        if (!_isInputAllowed) return;

        Character characterAtCursor = GetCharacterAtCurrentCell();
        if (characterAtCursor)
        {
            StartMove();
        }
        else
        {
            _camera.Grab(_cursor.Position);
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
        if (!_isInputAllowed) return;

        Character characterAtCursor = GetCharacterAtCurrentCell();
        if (CanMoveCharacter(characterAtCursor))
        {
            SetActiveCharacter(characterAtCursor);
            EazySoundManager.PlaySound(_moveStartClip);
        }
    }

    public void FinishMove()
    {
        if (!_activeCharacter) 
            return;

        bool shouldConfirm = _activeCharacter.CanStayInCell(_activeCharacter.CurrentCell);
        if (shouldConfirm)
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

        Character target = GetCharacterAtCurrentCell();
        if (!target || target == _activeCharacter)
        {
            _activeCharacter.Defend();
        }
        else if (target.Faction == _activeCharacter.Faction)
        {
            _activeCharacter.Heal(target);
        }
        else
        {
            _activeCharacter.Attack(target);
        }
        Deselect();
    }

    public void CancelMove()
    {
        if (!_activeCharacter)
            return;

        _activeCharacter.Cancel();
        Deselect();
    }

    private void Deselect()
    {
        SetHoveredCharacter(null);
        SetActiveCharacter(null);
    }

    private void SetCursorPosition(Vector2 newPosition)
    {
        if (_activeCharacter)
        {
            newPosition = _activeCharacter.ClampToReachableCells(newPosition);
        }
        _cursor.Position = newPosition;
    }

    private void SetActiveCharacterPosition(Vector2 newPosition)
    {
        if (!_activeCharacter)
            return;

        _activeCharacter.LookAt(newPosition);
        _activeCharacter.Position = _activeCharacter.ClampToStayableCells(newPosition);
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
            _hoveredCharacter.RefreshRange();
            _hoveredCharacter.ShowTransparentRange();
        }
    }

    private void SetActiveCharacter(Character activeCharacter)
    {
        if (activeCharacter == _activeCharacter) return;
        if (_activeCharacter)
        {
            _activeCharacter.HideRange();
        }        
        _activeCharacter = activeCharacter;
        _cursor.SetSelectionTarget(_activeCharacter ? _activeCharacter.transform : null);
        if (_activeCharacter)
        {
            SetHoveredCharacter(null);
            _activeCharacter.Position = _cursor.Position;
            _activeCharacter.SetIsRunning(true);
            _activeCharacter.ShowOpaqueRange();
        }
    }

    public Character GetCharacterAtCurrentCell()
    {
        return _battle.GetOccupant(_currentCell);
    }

    private void RefreshCurrentCell()
    {
        Vector2Int cell = _battle.WorldToCell(_cursor.Position);
        SetCurrentCell(cell);
    }

    private void SetCurrentCell(Vector2Int cell)
    {
        if (_currentCell == cell) return;

        _currentCell = cell;

        Vector2 worldPosition = _battle.CellToWorld(_currentCell);
        _gridReticle.MoveToPosition(worldPosition);
        if (_isInputAllowed)
        {
            _cellHoverAudio.Play();
        }
        RefreshHoveredCharacter();
        RefreshMovePreview();
    }

    private void RefreshHoveredCharacter()
    {
        if (!_activeCharacter)
        {
            SetHoveredCharacter(GetCharacterAtCurrentCell());
        }
    }

    private void RefreshMovePreview()
    {
        if (!_activeCharacter) return;

        Character characterAtCell = GetCharacterAtCurrentCell();
        if (characterAtCell && characterAtCell != _activeCharacter)
        {
            _activeCharacter.PreviewMove(characterAtCell);
        }
        else
        {
            _activeCharacter.ClearMovePreview();
        } 
    }

    private bool CanMoveCharacter(Character character)
    {
        return character && character.IsAbleToMove && character.Faction == _battle.CurrentFactionTurn;
    }

    private void RefreshIsInputAllowed()
    {
        SetIsInputAllowed(ShouldInputBeAllowed());
    }

    private bool ShouldInputBeAllowed()
    {
        return !_battle.IsTurnChangeAnimationPlaying && !Character.IsAnyCharacterActing && !BattleHistory.IsAnySequencePlaying;
    }

    private void SetIsInputAllowed(bool isInputAllowed)
    {
        if (isInputAllowed == _isInputAllowed) return;

        _isInputAllowed = isInputAllowed;
        if (isInputAllowed)
        {
            _cursor.Show();
            RefreshHoveredCharacter();
        }
        else
        {
            _cursor.Hide();
            Deselect();
        }
    }
}
