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

    protected virtual void Awake()
    {
        _battle = GetComponentInParent<Battle>();
    }

    public virtual bool IsAvailable() => true;
    public abstract Coroutine Execute();
}