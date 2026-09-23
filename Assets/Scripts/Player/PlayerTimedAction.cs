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

    protected virtual void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _ui = GetComponentInChildren<PlayerActionTimerUI>(true);
    }

    public bool Begin()
    {
        if (!IsAvailable())
            return false;

        Invoke(nameof(Confirm), Duration);
        _ui.Begin(this);
        return true;
    }

    public void Cancel()
    {
        CancelInvoke();
        _ui.CancelAnimation();
    }

    private void Confirm()
    {
        // TODO: wait on coroutine to check for rapid repeat
        Execute();
        _ui.PlayConfirmAnimation();
    }

    public virtual bool IsAvailable() => true;
    public abstract Coroutine Execute();
}