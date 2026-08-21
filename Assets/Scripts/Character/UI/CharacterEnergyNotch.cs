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

    private const float AnimationDuration = .35f;
    private const float DisappearScale = 5;

    [SerializeField] private SpriteRenderer _innerSprite;
    [SerializeField] private Color _filledColor = Color.yellow;
    [SerializeField] private Color _negativeColor = Color.red;
    [SerializeField, ReadOnly] private State _currentState = State.Invisible;

    private void Awake()
    {
        ReflectCurrentState();
    }

    public Coroutine AnimateState(State state)
    {
        if (state == _currentState)
            return null;

        State previous = _currentState;
        State next = state;
        _currentState = next;

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
            return transform.DOScale(1, AnimationDuration).WaitForCompletion();
        }
        else if (previous != State.Empty)
        {
            _innerSprite.DOFade(0, AnimationDuration);
            _innerSprite.transform.localScale = Vector3.one;
            return _innerSprite.transform.DOScale(DisappearScale, AnimationDuration).WaitForCompletion();
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
            return transform.DOScale(0, AnimationDuration).WaitForCompletion();
        }
        else if (next != State.Empty)
        {
            _innerSprite.DOFade(1, AnimationDuration);
            _innerSprite.transform.localScale = Vector3.one * DisappearScale;
            return _innerSprite.transform.DOScale(1, AnimationDuration).WaitForCompletion();
        }
        return null;
    }

    private void ReflectCurrentState()
    {
        transform.localScale = _currentState == State.Invisible ? Vector3.zero : Vector3.one;
        _innerSprite.transform.localScale = Vector3.one;
        _innerSprite.color = GetStateColor(_currentState);
    }

    private Color GetStateColor(State state)
    {
        return state == State.Filled ?
            _filledColor :
            state == State.Negative ?
            _negativeColor : Color.clear;
    }
}