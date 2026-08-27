using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class CharacterEnergyNotch : MonoBehaviour
{
    public enum State
    {
        Invisible, Filled, Empty, Negative
    }

    [SerializeField] private SpriteRenderer _frameSprite;
    [SerializeField] private SpriteRenderer _innerSprite;

    [Space]
    [SerializeField] private Color _filledColor = Color.yellow;
    [SerializeField] private Color _negativeColor = Color.red;
    [SerializeField] private float _animationDuration = .35f;
    [SerializeField] private float _disappearScale = 5;

    [Space]
    [SerializeField] private Color _emptyPreviewColor = Color.gray;
    [SerializeField] private float _previewFadeDuration = .3f;
    [SerializeField, ReadOnly] private State _currentState = State.Invisible;

    private void Awake()
    {
        ReflectCurrentState();
    }

    public void Preview(State state)
    {
        KillAllTweens();
        _innerSprite.color = GetPreviewStateColor(state);
        _innerSprite.DOFade(0, _previewFadeDuration).SetLoops(-1, LoopType.Yoyo);
    }

    public void ClearPreview()
    {
        ReflectCurrentState();
    }

    public Coroutine Animate(State state)
    {
        if (state == _currentState)
            return null;

        State previous = _currentState;
        State next = state;
        _currentState = next;

        KillAllTweens();
        StopAllCoroutines();
        return StartCoroutine(GetAnimationCoroutine(previous, next));
    }

    public void SetState(State state)
    {
        _currentState = state;
        ReflectCurrentState();
    }

    private IEnumerator GetAnimationCoroutine(State previous, State next)
    {
        yield return ExitStateTween(previous);
        yield return EnterStateTween(next);
        ReflectCurrentState();
    }

    private YieldInstruction ExitStateTween(State previous)
    {
        if (previous == State.Invisible)
        {
            _innerSprite.color = Color.clear;
            return transform.DOScale(1, _animationDuration).WaitForCompletion();
        }
        else if (previous != State.Empty)
        {
            _innerSprite.DOFade(0, _animationDuration);
            _innerSprite.transform.localScale = Vector3.one;
            return _innerSprite.transform.DOScale(_disappearScale, _animationDuration).WaitForCompletion();
        }
        return null;
    }

    private YieldInstruction EnterStateTween(State next)
    {
        Color color = GetStateColor(next);
        color.a = 0;
        _innerSprite.color = color;

        if (next == State.Invisible)
        {
            return transform.DOScale(0, _animationDuration).WaitForCompletion();
        }
        else if (next != State.Empty)
        {
            _innerSprite.DOFade(1, _animationDuration);
            _innerSprite.transform.localScale = Vector3.one * _disappearScale;
            return _innerSprite.transform.DOScale(1, _animationDuration).WaitForCompletion();
        }
        return null;
    }

    private void ReflectCurrentState()
    {
        KillAllTweens();
        transform.localScale = _currentState == State.Invisible ? Vector3.zero : Vector3.one;
        _innerSprite.transform.localScale = Vector3.one;
        _innerSprite.color = GetStateColor(_currentState);
    }

    private void KillAllTweens()
    {
        transform.DOKill();
        _innerSprite.DOKill();
        _innerSprite.transform.DOKill();
    }

    private Color GetPreviewStateColor(State state)
    {
        return state != State.Empty ? GetStateColor(state) : _emptyPreviewColor;
    }

    private Color GetStateColor(State state)
    {
        return state == State.Filled ?
            _filledColor :
            state == State.Negative ?
            _negativeColor : Color.clear;
    }
}