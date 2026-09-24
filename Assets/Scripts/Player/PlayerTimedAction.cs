using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
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
    private Player _player;
    private PlayerActionTimerUI _ui;
    private bool _isScheduled;
    private bool _isTriggered;

    protected virtual void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _player = GetComponent<Player>();
        _ui = GetComponentInChildren<PlayerActionTimerUI>(true);
    }

    public bool Begin()
    {
        return Begin(_duration);
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

    public virtual bool IsAvailable() => _player.IsInputAllowed;
    public abstract Coroutine Execute();

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

        yield return null;

        if (_isTriggered)
        {
            Begin(QuickRepeatDuration);
        }
    }
}