using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    public static event Action<Character> UsedMove = delegate { };

    public Vector2 Position
    {
        get => _rigidbody.position;
        set
        {
            _rigidbody.MovePosition(value);
        }
    }
    public Faction Faction => _faction;
    public bool HasMovedThisTurn => _hasMovedThisTurn;

    [SerializeField] private Faction _faction;
    [SerializeField] private Color _usedMoveFadeColor = Color.gray;
    [SerializeField] private float _usedMoveFadeDuration = 0.35f;
    private Rigidbody2D _rigidbody;
    private CharacterAnimator _animator;
    private SpriteRenderer _sprite;
    private Battlefield _battlefield;
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
        SetDirection(position - Position);
        StopAllCoroutines();
        StartCoroutine(GetRunToSequence(position, ease, duration));
    }

    public void UseMove()
    {
        SetHasMovedThisTurn(true);
    }

    public void RestoreMove()
    {
        SetHasMovedThisTurn(false);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<CharacterAnimator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (_battlefield)
        {
            _battlefield.Register(this);
        }
    }

    private void OnDisable()
    {
        if (_battlefield)
        {
            _battlefield.Unregister(this);
        }    
    }

    private void Start()
    {
        _battlefield = GetComponentInParent<Battlefield>();
        if (_battlefield)
        {
            _battlefield.Register(this);
        }
    }

    private IEnumerator GetRunToSequence(Vector2 target, Ease ease, float duration)
    {
        _rigidbody.DOMove(target, duration).SetEase(ease);
        SetIsRunning(true);
        yield return new WaitForSeconds(duration);
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
