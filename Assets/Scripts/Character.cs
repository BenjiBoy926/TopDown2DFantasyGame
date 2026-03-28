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

    public IReadOnlyCollection<Vector3Int> TraversibleTiles => _range.TraversibleTiles;
    public IReadOnlyCollection<Vector3Int> AttackableEdgeTiles => _range.AttackableEdgeTiles;
    public Vector2 Position
    {
        get => _rigidbody.position;
        set
        {
            _rigidbody.MovePosition(value);
        }
    }
    public Vector3Int HomeCell => _battle.GetCell(this);
    public Vector3Int CurrentCell => _battle.WorldToCell(Position);
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

    public void Wait(Vector3Int cell)
    {
        StopAllCoroutines();
        StartCoroutine(GetWaitSequence(cell));
    }

    public void UseMove()
    {
        SetHasMovedThisTurn(true);
    }

    public void RestoreMove()
    {
        SetHasMovedThisTurn(false);
    }

    public void RefreshRange()
    {
        _range.Refresh();
    }

    public Vector3Int ClosestTraversibleCell(Vector2 input)
    {
        return _range.ClosestTraversibleCell(input);
    }

    public void ShowRange()
    {
        if (_rangeDisplay.IsShown) return;

        RefreshRange();
        _rangeDisplay.Show();
    }

    public void HideRange()
    {
        if (!_rangeDisplay.IsShown) return;

        _rangeDisplay.Hide();
    }

    public Vector2 SnapToGrid(Vector3Int cell)
    {
        return _battle.SnapToGrid(cell);
    }

    public Vector3 CellToWorld(Vector3Int cell)
    {
        return _battle.CellToWorld(cell);
    }

    public Vector3Int WorldToCell(Vector3 position)
    {
        return _battle.WorldToCell(position);
    }

    public Vector2 ClampToTraversibleTiles(Vector2 position)
    {
        return _range.ClampToTraversibleTiles(position);
    }

    public bool IsTraversible(Vector3Int cell)
    {
        return Battlefield.RectangularDistance(HomeCell, cell) <= _traversalRange;
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

    private IEnumerator GetWaitSequence(Vector3Int cell)
    {
        Vector2 gridPosition = _battle.CellToWorld(cell);
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
