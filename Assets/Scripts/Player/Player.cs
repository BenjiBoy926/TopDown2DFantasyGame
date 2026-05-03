using Hellmade.Sound;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsCameraGrabbed => _camera.IsGrabbed;
    public Character ActiveCharacter => _activeCharacter;

    [SerializeField] private Transform _exactPosition;

    [Space]
    [SerializeField] private AudioSource _cellHoverAudio;

    [Space]
    [SerializeField] private AudioClip _moveStartClip;
    [SerializeField] private AudioClip _moveConfirmClip;
    [SerializeField] private AudioClip _moveCancelClip;

    private Battle _battle;
    private PlayerCamera _camera;
    private PlayerGridReticle _gridPosition;
    private Character _activeCharacter;
    private Character _hoveredCharacter;
    private Coroutine _characterActionRoutine;
    private Vector2Int _currentCell;
    private bool _isInputAllowed = true;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _camera = GetComponentInChildren<PlayerCamera>();
        _gridPosition = GetComponentInChildren<PlayerGridReticle>();
    }

    private void Update()
    {
        // There should be a way to refresh this less often, but since the refresh check is currently computationally cheap,
        // there is no need to make it more efficient yet
        RefreshIsInputAllowed();
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
            _activeCharacter.LookAt(newPosition);
            _activeCharacter.Position = _activeCharacter.ClampToTraversibleCells(newPosition);
            _exactPosition.position = _activeCharacter.ClampToReachableCells(newPosition);
        }
        else
        {
            _exactPosition.position = newPosition;
        }
        RefreshCurrentCell();
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

        Vector2Int targettingCell = _battle.WorldToCell(_exactPosition.position);
        Obstacle target = _battle.GetObstacle(targettingCell);
        bool shouldAttack = target && target.Character && target.Faction != _activeCharacter.Faction;
        Coroutine action = shouldAttack ? 
            _activeCharacter.Attack(target.Character) : 
            _activeCharacter.Defend();

        SetCharacterActionRoutine(action);
        Deselect();
        EazySoundManager.PlaySound(_moveConfirmClip);
    }

    public void CancelMove()
    {
        if (!_activeCharacter)
            return;

        Coroutine action = _activeCharacter.Cancel();
        SetCharacterActionRoutine(action);
        Deselect();
        EazySoundManager.PlaySound(_moveCancelClip);
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

        if (hoveredCharacter && !_isInputAllowed) return;

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

        if (activeCharacter && !_isInputAllowed) return;
        
        _activeCharacter = activeCharacter;
        if (_activeCharacter)
        {
            SetHoveredCharacter(null);
            _activeCharacter.Position = _exactPosition.position;
            _activeCharacter.SetIsRunning(true);
            _activeCharacter.ShowOpaqueRange();
        }
    }

    private Character GetCharacterAtCursor()
    {
        Obstacle obstacle = _battle.GetObstacle(_currentCell);
        return obstacle ? obstacle.Character : null;
    }

    private void RefreshCurrentCell()
    {
        Vector2Int cell = _battle.WorldToCell(_exactPosition.position);
        SetCurrentCell(cell);
    }

    private void SetCurrentCell(Vector2Int cell)
    {
        if (_currentCell == cell) return;

        _currentCell = cell;

        Vector2 worldPosition = _battle.CellToWorld(_currentCell);
        _gridPosition.MoveToPosition(worldPosition);
        if (_isInputAllowed)
        {
            _cellHoverAudio.Play();
        }
        RefreshHoveredCharacter();
    }

    private void RefreshHoveredCharacter()
    {
        if (!_activeCharacter)
        {
            SetHoveredCharacter(GetCharacterAtCursor());
        }
    }

    private bool CanMoveCharacter(Character character)
    {
        return character && character.IsAbleToMove && character.Faction == _battle.CurrentFactionTurn;
    }

    private void SetCharacterActionRoutine(Coroutine actionRoutine)
    {
        StopAllCoroutines();
        _characterActionRoutine = StartCoroutine(WaitForCharacterRoutine(actionRoutine));
    }

    private IEnumerator WaitForCharacterRoutine(Coroutine actionRoutine)
    {
        yield return actionRoutine;
        _characterActionRoutine = null;
    }

    private void RefreshIsInputAllowed()
    {
        SetIsInputAllowed(ShouldInputBeAllowed());
    }

    private bool ShouldInputBeAllowed()
    {
        return !_battle.IsTurnChangeAnimationPlaying && _characterActionRoutine == null;
    }

    private void SetIsInputAllowed(bool isInputAllowed)
    {
        if (isInputAllowed == _isInputAllowed) return;

        _isInputAllowed = isInputAllowed;
        _exactPosition.gameObject.SetActive(isInputAllowed);
        _gridPosition.gameObject.SetActive(isInputAllowed);
        if (isInputAllowed)
        {
            RefreshHoveredCharacter();
        }
        else
        {
            Deselect();
        }
    }
}
