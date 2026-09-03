using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterAppearance : MonoBehaviour
{
    public enum State
    {
        Default,
        CantMove,
        DeadAndGone
    }

    [SerializeField] private float _fadeDuration = .35f;
    private Character _character;
    private SpriteRenderer _renderer;
    private State _currentState = State.Default;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    public YieldInstruction FadeToTargetState()
    {
        return FadeToTargetState(_fadeDuration);
    }

    public YieldInstruction FadeToTargetState(float duration)
    {
        State targetState = GetTargetState();
        return FadeToState(targetState, duration);
    }

    private YieldInstruction FadeToState(State state, float duration)
    {
        if (state == _currentState)
            return null;

        _currentState = state;
        Color color = GetColor(state);

        _renderer.DOKill();
        return _renderer.DOColor(color, duration).WaitForCompletion();
    }

    private Color GetColor(State state)
    {
        return state switch
        {
            State.Default => Color.white,
            State.CantMove => Color.gray,
            _ => Color.clear
        };
    }

    private State GetTargetState()
    {
        if (_character.IsDead && !_character.CanBeRevived)
        {
            return State.DeadAndGone;
        }
        else if (_character.Faction == _character.CurrentFactionTurn && !_character.IsAbleToMove)
        {
            return State.CantMove;
        }
        else
        {
            return State.Default;
        }
    }
}