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
        PlayLoopingAnimation();
        _oneShotRoutine = null;
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
        return $"{_name}_{action}_{_verticalDirection}";
    }

    private void OnValidate()
    {
        PlayLoopingAnimation();   
    }
}
