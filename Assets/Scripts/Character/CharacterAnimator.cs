using UnityEngine;
using System.Collections;
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

    private const float OneShotProgressCheckInterval = 0.05f;
    private static readonly WaitForSeconds OneShotProgressCheckWait = new(OneShotProgressCheckInterval);

    public bool IsOneShotAnimationPlaying => _oneShotRoutine != null;

    [SerializeField] private HorizontalDirection _horizontalDirection;
    [SerializeField] private VerticalDirection _verticalDirection;
    [SerializeField] private bool _isRunning;
    [SerializeField] private int _hurtAnimationLoopCount = 5;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _oneShotRoutine;

    public void Pause()
    {
        _animator.speed = 0;
    }

    public void Resume()
    {
        _animator.speed = 1;
    }

    [Button]
    public Coroutine Attack()
    {
        return PlayOneShot(Actions.Attack);
    }

    [Button]
    public Coroutine Hurt()
    {
        return PlayOneShot(Actions.Hurt);
    }

    [Button]
    public Coroutine Die()
    {
        return PlayOneShot(Actions.Death);
    }

    public void SetDirection(Vector2 direction)
    {
        direction = direction.normalized;

        bool isHorizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
        bool isVertical = !isHorizontal;

        if (isHorizontal && direction.x > 0)
        {
            SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Right);
            SetVerticalDirection(CharacterAnimator.VerticalDirection.Side);
        }
        if (isHorizontal && direction.x < 0)
        {
            SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Left);
            SetVerticalDirection(CharacterAnimator.VerticalDirection.Side);
        }
        if (isVertical && direction.y > 0)
        {
            SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Right);
            SetVerticalDirection(CharacterAnimator.VerticalDirection.Up);
        }
        if (isVertical && direction.y < 0)
        {
            SetHorizontalDirection(CharacterAnimator.HorizontalDirection.Right);
            SetVerticalDirection(CharacterAnimator.VerticalDirection.Down);
        }
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

    public void PlayLoopingAnimation()
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

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        PlayLoopingAnimation();
    }

    private Coroutine PlayOneShot(Actions action)
    {
        if (_oneShotRoutine != null)
        {
            StopCoroutine(_oneShotRoutine);
        }
        _oneShotRoutine = StartCoroutine(PlayOneShotRoutine(action));
        return _oneShotRoutine;
    }

    private IEnumerator PlayOneShotRoutine(Actions action)
    {
        Play(action);
        int loopCount = action == Actions.Hurt ? _hurtAnimationLoopCount : 1;
        yield return WaitForAnimationLoopCount(action, loopCount);
        _oneShotRoutine = null;
    }

    private IEnumerator WaitForAnimationLoopCount(Actions action, int loops)
    {
        AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
        int nameHash = GetStateHash(action);
        while (currentState.shortNameHash != nameHash || currentState.normalizedTime < loops)
        {
            yield return OneShotProgressCheckWait;
            currentState = _animator.GetCurrentAnimatorStateInfo(0);
        }
    }

    private void RefreshLoopingAnimation()
    {
        if (!IsOneShotAnimationPlaying)
        {
            PlayLoopingAnimation();
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

    private int GetStateHash(Actions action)
    {
        return Animator.StringToHash(GetStateName(action));
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
