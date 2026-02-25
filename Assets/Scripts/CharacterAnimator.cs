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

    private const float OneShotEstimatedAnimationDuration = 0.35f;
    private static readonly YieldInstruction OneShotAnimationWait = new WaitForSeconds(OneShotEstimatedAnimationDuration);

    public bool IsOneShotAnimationPlaying => _oneShotRoutine != null;

    [SerializeField] private string _name = "Swordman";
    [SerializeField] private HorizontalDirection _horizontalDirection;
    [SerializeField] private VerticalDirection _verticalDirection;
    [SerializeField] private bool _isRunning;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _oneShotRoutine;

    [Button]
    public void Attack()
    {
        PlayOneShot(Actions.Attack);
    }

    [Button]
    public void Hurt()
    {
        PlayOneShot(Actions.Hurt);
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

    private void PlayOneShot(Actions action)
    {
        if (_oneShotRoutine != null) return;
        _oneShotRoutine = StartCoroutine(PlayOneShotRoutine(action));
    }

    private IEnumerator PlayOneShotRoutine(Actions action)
    {
        Play(action);
        yield return OneShotAnimationWait;
        _oneShotRoutine = null;
        RefreshLoopingAnimation();
    }

    private void RefreshLoopingAnimation()
    {
        if (!IsOneShotAnimationPlaying)
        {
            PlayLoopingAnimation();
        }
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
        string stateName = $"{_name}_{action}";
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
