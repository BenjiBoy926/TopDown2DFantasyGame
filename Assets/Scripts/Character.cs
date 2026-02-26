using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    private const float DirectionChangeThreshold = 0.001f;

    public Vector2 Position
    {
        get => _rigidbody.position;
        set
        {
            _rigidbody.MovePosition(value);
        }
    }

    [SerializeField] private Vector2 _direction;
    private Rigidbody2D _rigidbody;
    private CharacterAnimator _animator;
    private float _timeToStopRunning;

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
        RefreshAnimatorDirection();
    }

    public void Attack()
    {
        _rigidbody.velocity = Vector2.zero;
        _animator.Attack();
    }

    public void RunForTime(float time)
    {
        _timeToStopRunning = Time.time + time;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<CharacterAnimator>();
    }

    private void Update()
    {
        if (_animator.IsOneShotAnimationPlaying) return;

        _animator.SetIsRunning(Time.time < _timeToStopRunning);
    }

    private void RefreshAnimatorDirection()
    {
        if (Mathf.Abs(_direction.y) > DirectionChangeThreshold || _direction.x > DirectionChangeThreshold)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Right);
        }
        else
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Left);
        }

        bool isMovingHorizontally = Mathf.Abs(_direction.x) > DirectionChangeThreshold;
        if (_direction.y > DirectionChangeThreshold)
        {
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Up);
        }
        else if (_direction.y < -DirectionChangeThreshold)
        {
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Down);
        }
        else if (isMovingHorizontally)
        {
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Side);
        }
    }
}
