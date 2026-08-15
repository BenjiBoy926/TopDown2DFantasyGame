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
        gameObject.SetActive(state != State.Invisible);
        return null;
    }

    public void SetState(State state)
    {
        _currentState = state;
        ReflectCurrentState();
    }

    private void ReflectCurrentState()
    {
        gameObject.SetActive(_currentState != State.Invisible);
        switch (_currentState)
        {
            case State.Filled:
                _innerSprite.transform.localScale = Vector3.one;
                _innerSprite.color = _filledColor;
                break;
            case State.Empty:
                _innerSprite.color = Color.clear;
                break;
            case State.Negative:
                _innerSprite.transform.localScale = Vector3.one;
                _innerSprite.color = _negativeColor;
                break;
        }
    }

    private IEnumerator GetAnimationCoroutine(State state)
    {
        State previous = _currentState;
        State next = state;

        if (previous == State.Invisible)
        {
            _innerSprite.color = Color.clear;
            yield return transform.DOScale(1, .2f).WaitForCompletion();
        }
        else if (previous == State.Filled || previous == State.Negative)
        {
            _innerSprite.DOFade(0, .2f);
            yield return _innerSprite.transform.DOScale(2, .2f).WaitForCompletion();
        }

        ReflectCurrentState();
    }

    private Color GetStateColor(State state)
    {
        return state == State.Filled ? 
            _filledColor : 
            state == State.Negative ? 
            _negativeColor : Color.clear;
    }
}