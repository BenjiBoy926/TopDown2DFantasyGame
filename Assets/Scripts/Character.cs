using UnityEngine;

[RequireComponent(typeof(CharacterAnimator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    private const float DirectionChangeThreshold = 0.001f;

    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _speed = 5;
    private CharacterAnimator _animator;
    private Rigidbody2D _rigidbody;

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;

        if (direction.x > DirectionChangeThreshold)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Right);
        }
        if (direction.x < -DirectionChangeThreshold)
        {
            _animator.SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Left);
        }

        if (direction.y > DirectionChangeThreshold)
        {
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Up);
        }
        else if (direction.y < -DirectionChangeThreshold)
        {
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Down);
        }
        else
        {
            _animator.SetVerticalDirection(CharacterAnimator.VerticalDirection.Side);
        }

        _animator.SetIsRunning(direction.sqrMagnitude > DirectionChangeThreshold);

        _rigidbody.velocity = direction * _speed;
    }

    private void Awake()
    {
        _animator = GetComponent<CharacterAnimator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
}
