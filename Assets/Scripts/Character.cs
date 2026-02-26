using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    private const float AngleChangeThreshold = 15f;

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
        _direction = direction.normalized;
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
        float rightAngle = Vector2.Angle(_direction, Vector2.right);
        float leftAngle = Vector2.Angle(_direction, Vector2.left);
        float upAngle = Vector2.Angle(_direction, Vector2.up);
        float downAngle = Vector2.Angle(_direction, Vector2.down);

        if (rightAngle < AngleChangeThreshold)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Right);
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Side);
        }
        if (leftAngle < AngleChangeThreshold)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Left);
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Side);
        }
        if (upAngle < AngleChangeThreshold)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Right);
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Up);
        }
        if (downAngle < AngleChangeThreshold)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Right);
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Down);
        }
    }
}
