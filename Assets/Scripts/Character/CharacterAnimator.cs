using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterAnimator : MonoBehaviour
{
    public enum HorizontalDirection
    {
        Left, Right
    }
    public enum VerticalDirection
    {
        Up, Side, Down
    }
    public enum Actions
    {
        Idle, Run, Attack, Hurt, Death
    }

    [SerializeField] private HorizontalDirection _horizontalDirection;
    [SerializeField] private VerticalDirection _verticalDirection;
    [SerializeField] private bool _isRunning;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [Button]
    public void Attack()
    {
        Play(Actions.Attack);
    }

    [Button]
    public void Hurt()
    {
        Play(Actions.Hurt);
    }

    [Button]
    public void Die()
    {
        Play(Actions.Death);
    }

    public void SetHorizontalDirection(HorizontalDirection horizontalDirection)
    {
        if (_horizontalDirection == horizontalDirection) return;
        _horizontalDirection = horizontalDirection;
        RefreshLoopingAnimation();
    }

    public void SetVerticalDirection(VerticalDirection verticalDirection)
    {
        if (_verticalDirection == verticalDirection) return;
        _verticalDirection = verticalDirection;
        RefreshLoopingAnimation();
    }

    public void SetIsRunning(bool isRunning)
    {
        if (_isRunning == isRunning) return;
        _isRunning = isRunning;
        RefreshLoopingAnimation();
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        PlayLoopingAnimation();
    }

    private void RefreshLoopingAnimation()
    {
        PlayLoopingAnimation();
    }

    private void PlayLoopingAnimation()
    {
        if (_spriteRenderer)
        {
            _spriteRenderer.flipX = _horizontalDirection == HorizontalDirection.Left;
        }
        if (_animator)
        {
            Actions action = _isRunning ? Actions.Run : Actions.Idle;
            Play(action);
        }
    }

    private void Play(Actions action)
    {
        string fullStateName = GetFullStateName(action);
        _animator.Play(fullStateName);
    }

    private string GetFullStateName(Actions action)
    {
        return $"Base Layer.{GetStateName(action)}";
    }

    private string GetStateName(Actions action)
    {
        string stateName = $"{action}";
        if (action != Actions.Death)
        {
            stateName += $"_{_verticalDirection}";
        }
        return stateName;
    }

    private void OnValidate()
    {
        RefreshLoopingAnimation();
    }
}
