using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    private const float AngleChangeThreshold = 45f;

    public Vector2 Position
    {
        get => _rigidbody.position;
        set
        {
            _rigidbody.MovePosition(value);
        }
    }

    private Rigidbody2D _rigidbody;
    private CharacterAnimator _animator;

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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<CharacterAnimator>();
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
        float rightAngle = Vector2.Angle(direction, Vector2.right);
        float leftAngle = Vector2.Angle(direction, Vector2.left);
        float upAngle = Vector2.Angle(direction, Vector2.up);
        float downAngle = Vector2.Angle(direction, Vector2.down);

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
