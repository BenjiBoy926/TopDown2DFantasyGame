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
        if (previous == State.Invisible)
        {
            _innerSprite.color = Color.clear;
            yield return transform.DOScale(1, .2f).WaitForCompletion();
        }
        else if (previous != State.Empty)
        {
            _innerSprite.DOFade(0, .2f);
            _innerSprite.transform.localScale = Vector3.one;
            yield return _innerSprite.transform.DOScale(2, .2f).WaitForCompletion();
        }

        _innerSprite.color = GetStateColor(next);
        if (next == State.Invisible)
        {
            yield return transform.DOScale(0, .2f).WaitForCompletion();
        }
        else if (next != State.Empty)
        {
            _innerSprite.DOFade(1, .2f);
            _innerSprite.transform.localScale = Vector3.one * 2;
            yield return _innerSprite.transform.DOScale(1, .2f).WaitForCompletion();
        }

        ReflectCurrentState();
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