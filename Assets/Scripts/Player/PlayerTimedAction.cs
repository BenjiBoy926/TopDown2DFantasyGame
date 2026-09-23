using System.Collections;
using UnityEngine;

public abstract class PlayerTimedAction : MonoBehaviour
{
    private const float QuickRepeatDuration = 0.01f;
    private const float QuickRepeatWindow = .35f;

    public float Duration => _duration;
    public Sprite Sprite => _sprite;
    public abstract string Label { get; }
    protected Battle Battle => _battle;

    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _duration;
    private Battle _battle;
    private PlayerActionTimerUI _ui;
    private bool _isTriggered;

    protected virtual void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _ui = GetComponentInChildren<PlayerActionTimerUI>(true);
    }

    public bool Begin()
    {
        return Begin(_duration);
    }

    public void Cancel()
    {
        StopAllCoroutines();
        _ui.CancelAnimation();
        _isTriggered = false;
    }

    public virtual bool IsAvailable() => true;
    public abstract Coroutine Execute();

    private bool Begin(float duration)
    {
        if (!IsAvailable())
            return false;

        StartCoroutine(ExecutionSequence(duration));
        _ui.Begin(this, duration);
        _isTriggered = true;
        return true;
    }

    private IEnumerator ExecutionSequence(float duration)
    {
        yield return new WaitForSeconds(duration);
        _ui.PlayConfirmAnimation();
        yield return Execute();

        // TODO: this does not work because "_isTriggered" is not set back to false
        // if the Player has inputs disabled (such as during an undo/redo)
        //if (_isTriggered)
        //{
        //    Begin(QuickRepeatDuration);
        //}
    }
}