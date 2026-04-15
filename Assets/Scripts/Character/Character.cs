using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterRange))]
[RequireComponent(typeof(CharacterRangeDisplay))]
[RequireComponent(typeof(CharacterAttackBehaviour))]
[RequireComponent(typeof(CharacterDefendBehaviour))]
[RequireComponent(typeof(CharacterCancelBehaviour))]
[RequireComponent(typeof(CharacterStats))]
public class Character : MonoBehaviour
{
    public static event Action<Character> MoveFinished = delegate { };

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
    public int Power => _stats.Power;

    [SerializeField] private Faction _faction;
    [SerializeField] private int _traversalRange = 3;
    [SerializeField] private Color _usedMoveFadeColor = Color.gray;
    [SerializeField] private float _usedMoveFadeDuration = 0.35f;
    [SerializeField] private Ease _runEase = Ease.OutCirc;
    [SerializeField] private float _runDuration = 0.35f;
    private Rigidbody2D _rigidbody;
    private CharacterAnimator _animator;
    private SpriteRenderer _sprite;
    private CharacterRange _range;
    private CharacterRangeDisplay _rangeDisplay;
    private CharacterAttackBehaviour _attackBehaviour;
    private CharacterDefendBehaviour _defendBehaviour;
    private CharacterCancelBehaviour _cancelBehaviour;
    private CharacterStats _stats;
    private Battle _battle;
    private bool _hasMovedThisTurn = false;

    public void LookAt(Vector2 position)
    {
        Vector2 direction = position - Position;
        SetDirection(direction);
    }

    public void SetDirection(Vector2 direction)
    {
        RefreshAnimatorDirection(direction);
    }

    public Coroutine PlayAttackAnimation()
    {
        return _animator.Attack();
    }

    public Coroutine PlayHurtAnimation()
    {
        return _animator.Hurt();
    }

    public void SetIsRunning(bool isRunning)
    {
        _animator.SetIsRunning(isRunning);
    }

    public void Attack(Character other)
    {
        StopAllCoroutines();
        StartCoroutine(_attackBehaviour.GetSequence(other));
    }

    public void Defend()
    {
        StopAllCoroutines();
        StartCoroutine(_defendBehaviour.GetSequence());
    }

    public void Cancel()
    { 
        StopAllCoroutines();
        StartCoroutine(_cancelBehaviour.GetSequence());
    }

    public void SecureCurrentCell()
    {
        UseMove();
        _battle.RefreshOccupantCell(this);
    }

    public void UseMove()
    {
        SetHasMovedThisTurn(true);
    }

    public void RestoreMove()
    {
        SetHasMovedThisTurn(false);
        PerformSpriteFade();
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

    public IEnumerator WaitInCurrentCell()
    {
        Vector2 gridPosition = _battle.CellToWorld(CurrentCell);
        yield return GetRunToSequence(gridPosition);
        yield return PerformSpriteFade();
        MoveFinished.Invoke(this);
    }

    public IEnumerator GetRunToSequence(Vector2 target)
    {
        SetIsRunning(true);
        yield return _rigidbody.DOMove(target, _runDuration).SetEase(_runEase).WaitForCompletion();
        SetIsRunning(false);
    }

    public void TakeDamageFrom(Character other)
    {
        _stats.TakeDamageFrom(other);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<CharacterAnimator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _range = GetComponent<CharacterRange>();
        _rangeDisplay = GetComponent<CharacterRangeDisplay>();
        _attackBehaviour = GetComponent<CharacterAttackBehaviour>();
        _defendBehaviour = GetComponent<CharacterDefendBehaviour>();
        _cancelBehaviour = GetComponent<CharacterCancelBehaviour>();
        _stats = GetComponent<CharacterStats>();
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
        _hasMovedThisTurn = hasMovedThisTurn;
    }

    private YieldInstruction PerformSpriteFade()
    {
        _sprite.DOKill();
        if (_hasMovedThisTurn)
        {
            return _sprite.DOColor(_usedMoveFadeColor, _usedMoveFadeDuration).WaitForCompletion();
        }
        else
        {
            return _sprite.DOColor(Color.white, _usedMoveFadeDuration).WaitForCompletion();
        }
    }
}
