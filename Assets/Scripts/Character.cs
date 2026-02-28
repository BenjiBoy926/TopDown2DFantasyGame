using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
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
}
