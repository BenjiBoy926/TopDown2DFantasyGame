using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public abstract class PlayerTimedAction : MonoBehaviour
{
    private const float QuickRepeatMinimumDuration = .01f;

    public Sprite Sprite => _sprite;
    public abstract string Label { get; }
    protected Battle Battle => _battle;

    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _duration;
    [SerializeField] private float _quickRepeatScalar = .5f;
    [SerializeField] private float _quickRepeatWindow = .35f;
    private Battle _battle;
    private Player _player;
    private PlayerActionTimerUI _ui;
    private bool _isScheduled;
    private bool _isTriggered;
    private float _timeOfExecutionFinished;
    private int _quickRepeatCount = 0;

    protected virtual void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _player = GetComponent<Player>();
        _ui = GetComponentInChildren<PlayerActionTimerUI>(true);
    }

    public bool Begin()
    {
        bool isInQuickRepeatWindow = (Time.time - _timeOfExecutionFinished) <= _quickRepeatWindow;
        bool shouldQuickRepeat = isInQuickRepeatWindow && IsQuickRepeatAvailable();
        if (shouldQuickRepeat)
        {
            return BeginQuickRepeat();
        }
        else
        {
            _quickRepeatCount = 0;
            return Begin(_duration);
        }
    }

    private bool BeginQuickRepeat()
    {
        _quickRepeatCount++;
        float duration = _duration * Mathf.Pow(_quickRepeatScalar, _quickRepeatCount);
        duration = Mathf.Max(duration, QuickRepeatMinimumDuration);
        return Begin(duration);
    }

    public void Cancel()
    {
        _isTriggered = false;
        if (_isScheduled)
        {
            StopAllCoroutines();
            _ui.CancelAnimation();
        }
    }

    private bool Begin(float duration)
    {
        _isTriggered = true;
        if (!IsAvailable())
            return false;

        StartCoroutine(ExecutionSequence(duration));
        _ui.Begin(this, duration);
        return true;
    }

    private IEnumerator ExecutionSequence(float duration)
    {
        _isScheduled = true;
        yield return new WaitForSeconds(duration);
        _isScheduled = false;

        _ui.PlayConfirmAnimation();
        yield return Execute();

        // Wait for Player.IsInputAllowed to update during the frame after execution finishes
        yield return null;

        _timeOfExecutionFinished = Time.time;
        if (_isTriggered)
        {
            Begin();
        }
    }

    public virtual bool IsQuickRepeatAvailable() => false;
    public virtual bool IsAvailable() => _player.IsInputAllowed;
    public abstract Coroutine Execute();
}