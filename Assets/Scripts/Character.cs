using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterRange))]
[RequireComponent(typeof(CharacterRangeDisplay))]
public class Character : MonoBehaviour
{
    public static event Action<Character> UsedMove = delegate { };

    public IReadOnlyCollection<Vector2Int> TraversibleCells => _range.TraversibleCells;
    public IReadOnlyCollection<Vector2Int> AttackableEdgeCells => _range.AttackableEdgeCells;
    public Vector2 Position
    {
        get => _rigidbody.position;
        set
        {
            _rigidbody.MovePosition(value);
        }
    }
    public Vector2Int HomeCell => _battle.GetCell(this);
    public Vector2Int CurrentCell => _battle.WorldToCell(Position);
    public Faction Faction => _faction;
    public bool HasMovedThisTurn => _hasMovedThisTurn;
    public float CellWidth => _battle.CellWidth;
    public float CellHeight => _battle.CellHeight;

    [SerializeField] private Faction _faction;
    [SerializeField] private Color _usedMoveFadeColor = Color.gray;
    [SerializeField] private float _usedMoveFadeDuration = 0.35f;
    [SerializeField] private int _traversalRange = 3;
    private Rigidbody2D _rigidbody;
    private CharacterAnimator _animator;
    private SpriteRenderer _sprite;
    private CharacterRange _range;
    private CharacterRangeDisplay _rangeDisplay;
    private Battle _battle;
    
    private bool _hasMovedThisTurn = false;

    public void SetDirection(Vector2 direction)
    {
        RefreshAnimatorDirection(direction);
    }

    public void Attack()
    {
        _rigidbody.velocity = Vector2.zero;
        _animator.Attack();
    }

    public void SetIsRunning(bool isRunning)
    {
        _animator.SetIsRunning(isRunning);
    }

    public void RunTo(Vector2 position, Ease ease, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(GetRunToSequence(position, ease, duration));
    }

    public void Wait()
    {
        StopAllCoroutines();
        StartCoroutine(GetWaitSequence());
    }

    public void UseMove()
    {
        SetHasMovedThisTurn(true);
    }

    public void RestoreMove()
    {
        SetHasMovedThisTurn(false);
    }

    public Vector2 ClampToReachableCells(Vector2 input)
    {
        return _range.ClampToReachableCells(input);
    }

    public void ShowRange()
    {
        if (_rangeDisplay.IsShown) return;

        _range.Refresh();
        _rangeDisplay.Show();
    }

    public void HideRange()
    {
        if (!_rangeDisplay.IsShown) return;

        _rangeDisplay.Hide();
    }

    public Vector2 SnapToGrid(Vector2Int cell)
    {
        return _battle.SnapToGrid(cell);
    }

    public Vector2 CellToWorld(Vector2Int cell)
    {
        return _battle.CellToWorld(cell);
    }

    public Vector2Int WorldToCell(Vector2 position)
    {
        return _battle.WorldToCell(position);
    }

    public Vector2 ClampToTraversibleCells(Vector2 position)
    {
        return _range.ClampToTraversibleCells(position);
    }

    public bool IsTraversible(CellCost cell)
    {
        if (cell.CostToArrive > _traversalRange)
        {
            return false;
        }
        Character occupant = _battle.GetOccupant(cell.Cell);
        bool canMoveThroughOccupant = !occupant || occupant.Faction == _faction;
        return canMoveThroughOccupant;
    }

    public bool CanStayInCell(Vector2Int cell)
    {
        Character occupant = _battle.GetOccupant(cell);
        return !occupant || occupant == this;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<CharacterAnimator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _range = GetComponent<CharacterRange>();
        _rangeDisplay = GetComponent<CharacterRangeDisplay>();
    }

    private void OnEnable()
    {
        if (_battle)
        {
            _battle.Register(this);
        }
    }

    private void OnDisable()
    {
        if (_battle)
        {
            _battle.Unregister(this);
        }    
    }

    private void Start()
    {
        _battle = GetComponentInParent<Battle>();
        if (_battle)
        {
            _battle.Register(this);
        }
    }

    private IEnumerator GetWaitSequence()
    {
        Vector2 gridPosition = _battle.CellToWorld(CurrentCell);
        _battle.RefreshOccupantCell(this);
        yield return GetRunToSequence(gridPosition, Ease.OutCirc, 0.35f);
        UseMove();
    }

    private IEnumerator GetRunToSequence(Vector2 target, Ease ease, float duration)
    {
        SetDirection(target - Position);
        SetIsRunning(true);
        yield return _rigidbody.DOMove(target, duration).SetEase(ease);
        SetIsRunning(false);
    }

    private void RefreshAnimatorDirection(Vector2 direction)
    {
        direction = direction.normalized;

        bool isHorizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
        bool isVertical = !isHorizontal;

        if (isHorizontal && direction.x > 0)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Right);
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Side);
        }
        if (isHorizontal && direction.x < 0)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Left);
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Side);
        }
        if (isVertical && direction.y > 0)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Right);
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Up);
        }
        if (isVertical && direction.y < 0)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Right);
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Down);
        }
    }

    private void SetHasMovedThisTurn(bool hasMovedThisTurn)
    {
        if (_hasMovedThisTurn == hasMovedThisTurn) return;

        _hasMovedThisTurn = hasMovedThisTurn;
        if (hasMovedThisTurn)
        {
            _sprite.DOColor(_usedMoveFadeColor, _usedMoveFadeDuration);
            UsedMove(this);
        }
        else
        {
            _sprite.DOColor(Color.white, _usedMoveFadeDuration);
        }
    }
}
