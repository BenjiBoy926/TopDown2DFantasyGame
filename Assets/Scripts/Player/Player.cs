using Hellmade.Sound;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RangeWarningSystem))]
public class Player : MonoBehaviour
{
    public Character ActiveCharacter => _activeCharacter;
    public int ActiveCharacterRange => _activeCharacter.TraversalRange;
    public Faction Faction => _faction;
    public Transform CommanderTransform => _faction.CommanderTransform;
    public Vector3 CommanderPosition => _faction.CommanderPosition;
    public bool IsInputAllowed => _isInputAllowed;
    public HashSet<Character> AllCharacters => _battle.AllCharacters;

    [SerializeField] private Faction _faction;
    [SerializeField] private AudioSource _cellHoverAudio;
    [SerializeField] private AudioClip _moveStartClip;

    private Battle _battle;
    private PlayerCursor _cursor;
    private PlayerGridReticle _gridReticle;
    private RangeWarningSystem _rangeWarning;
    private Character _activeCharacter;
    private Character _hoveredCharacter;
    private Vector2Int _currentCell;
    private bool _isInputAllowed = true;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _cursor = GetComponentInChildren<PlayerCursor>();
        _gridReticle = GetComponentInChildren<PlayerGridReticle>();
        _rangeWarning = GetComponent<RangeWarningSystem>();
    }

    private void Update()
    {
        RefreshIsInputAllowed();
    }

    public void StartNextTurn()
    {
        _battle.StartNextTurn();
    }

    public void Undo()
    {
        _battle.Undo();
    }

    public void Redo()
    {
        _battle.Redo();
    }

    public void IncludeInView()
    {
        _battle.IncludeInView(_cursor.Position);
    }

    public void ChangeZoom(float zoom)
    {
        _battle.ChangeZoom(zoom);
    }

    public void ZoomIn()
    {
        _battle.ZoomIn();
    }

    public void ZoomOut()
    {
        _battle.ZoomOut();
    }

    public void SlidePosition(Vector2 offset)
    {
        Vector2 Position = _cursor.Position;
        SetPosition(Position + offset);
    }

    public void SetScreenPosition(Vector2 screenPosition)
    {
        if (_battle.IsCameraGrabbed)
        {
            _battle.UpdateCameraGrab(screenPosition);
        }
        else
        {
            Vector2 worldPosition = _battle.ScreenToWorld(screenPosition);
            SetPosition(worldPosition);
        }
    }

    public void SetPosition(Vector2 newPosition)
    {
        SetActiveCharacterPosition(newPosition);
        SetCursorPosition(newPosition);
        RefreshCurrentCell();
    }

    public void Grab()
    {
        Character characterAtCursor = GetCharacterAtCurrentCell();
        if (characterAtCursor)
        {
            StartMove();
        }
        else
        {
            _battle.GrabCamera(_cursor.Position);
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
            _battle.ReleaseCamera();
        }
    }

    public void StartMove()
    {
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

        bool shouldConfirm = _activeCharacter.CouldStayInCell(_activeCharacter.CurrentCell);
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
        _activeCharacter.RefreshCell();
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
        RefreshWarningSystem();
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
        RefreshWarningSystem();
        if (_activeCharacter)
        {
            SetHoveredCharacter(null);
            _activeCharacter.Position = _cursor.Position;
            _activeCharacter.SetIsRunning(true);
            _activeCharacter.ShowOpaqueRange();
        }
    }

    private void RefreshWarningSystem()
    {
        if (_hoveredCharacter)
        {
            _rangeWarning.SetTarget(_hoveredCharacter);
        }
        else if (_activeCharacter)
        {
            _rangeWarning.SetTarget(_activeCharacter);
        }
        else
        {
            _rangeWarning.SetTarget(null);
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
        if (!_activeCharacter && _isInputAllowed)
        {
            SetHoveredCharacter(GetCharacterAtCurrentCell());
        }
    }

    private void RefreshMovePreview()
    {
        if (!_activeCharacter) 
            return;

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
        return character && character.IsAbleToMove && character.Faction == _faction;
    }

    private void RefreshIsInputAllowed()
    {
        SetIsInputAllowed(ShouldInputBeAllowed());
    }

    private bool ShouldInputBeAllowed()
    {
        return !_battle.IsTurnChangeAnimationPlaying && !Character.IsAnyCharacterActing && !BattleHistory.IsAnySequencePlaying && _battle.CurrentFactionTurn == _faction;
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
