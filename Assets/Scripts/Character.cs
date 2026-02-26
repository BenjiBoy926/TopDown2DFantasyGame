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
    [SerializeField] private float _speed = 5;
    private Rigidbody2D _rigidbody;
    private CharacterAnimator _animator;

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    public void Attack()
    {
        _rigidbody.velocity = Vector2.zero;
        _animator.Attack();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<CharacterAnimator>();
    }

    private void Update()
    {
        if (_animator.IsOneShotAnimationPlaying) return;

        UpdateHorizontalDirection();
        UpdateVerticalDirection();
        _animator.SetIsRunning(_direction.sqrMagnitude > DirectionChangeThreshold);
    }

    private void UpdateHorizontalDirection()
    {
        if (_direction.x > DirectionChangeThreshold)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Right);
        }
        else if (_direction.x < -DirectionChangeThreshold)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Left);
        }
    }

    private void UpdateVerticalDirection()
    {
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
