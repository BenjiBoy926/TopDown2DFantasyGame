using UnityEngine;

public class PlayerUndoTimer : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;
    private float _startTime = 0;
    private Battle _battle;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
    }

    public void Begin()
    {
        _startTime = Time.time;
        enabled = true;
    }

    private void Update()
    {
        float elapsedTime = Time.time - _startTime;
        if (elapsedTime >= _duration)
        {
            Trigger();
        }
    }

    public void End()
    {
        enabled = false;
    }

    private void Trigger()
    {
        _battle.Undo();
        enabled = false;
    }
}