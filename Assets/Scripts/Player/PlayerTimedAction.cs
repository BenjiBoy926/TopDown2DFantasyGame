using System.Collections;
using UnityEngine;

public abstract class PlayerTimedAction : MonoBehaviour
{
    public float Duration => _duration;
    public Sprite Sprite => _sprite;
    public abstract string Label { get; }
    protected Battle Battle => _battle;

    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _duration;
    private Battle _battle;
    private PlayerActionTimerUI _ui;
    private bool _isScheduled;
    private bool _isRunning;

    protected virtual void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _ui = GetComponentInChildren<PlayerActionTimerUI>(true);
    }

    public bool Begin()
    {
        if (!IsAvailable())
            return false;

        StartCoroutine(ExecutionSequence());
        _ui.Begin(this);
        return true;
    }

    public void Cancel()
    {
        StopAllCoroutines();
        _ui.CancelAnimation();

        _isScheduled = false;
        _isRunning = false; // Not perfectly accurate because we do not stop THAT coroutine, but not a bug right now either
    }

    public virtual bool IsAvailable() => true;
    public abstract Coroutine Execute();

    private IEnumerator ExecutionSequence()
    {
        _isScheduled = true;
        yield return new WaitForSeconds(_duration);
        _isScheduled = false;

        _ui.PlayConfirmAnimation();

        _isRunning = true;
        yield return Execute();
        _isRunning = false;
    }
}