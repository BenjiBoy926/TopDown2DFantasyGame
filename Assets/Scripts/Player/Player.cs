using Hellmade.Sound;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    private bool IsInputAllowed => !_battle.IsTurnChangeAnimationPlaying && _characterActionRoutine == null;
    public bool IsCameraGrabbed => _camera.IsGrabbed;
    public Character ActiveCharacter => _activeCharacter;

    [SerializeField] private Transform _exactPosition;
    [SerializeField] private Transform _gridPosition;
    [SerializeField] private PlayerCamera _camera;

    [Space]
    [SerializeField] private AudioClip _cellHoverClip;
    [SerializeField] private float _cellHoverVolume = .1f;

    [Space]
    [SerializeField] private AudioClip _moveStartClip;
    [SerializeField] private AudioClip _moveConfirmClip;
    [SerializeField] private AudioClip _moveCancelClip;

    private Battle _battle;
    private Character _activeCharacter;
    private Character _hoveredCharacter;
    private Coroutine _characterActionRoutine;
    private Vector2Int _currentCell;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
    }

    private void Update()
    {
        bool isInputAllowed = IsInputAllowed;
        _exactPosition.gameObject.SetActive(isInputAllowed);
        _gridPosition.gameObject.SetActive(isInputAllowed);
        if (!isInputAllowed)
        {
            Deselect();
        }
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

    private void UpdateHoveredCharacter()
    {
        if (!_activeCharacter)
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

        if (hoveredCharacter && !IsInputAllowed) return;

        _hoveredCharacter = hoveredCharacter;
        if (_hoveredCharacter)
        {
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

        if (activeCharacter && !IsInputAllowed) return;
        
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
        _gridPosition.position = _battle.CellToWorld(_currentCell);
        if (IsInputAllowed)
        {
            EazySoundManager.PlaySound(_cellHoverClip, _cellHoverVolume);
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
}
