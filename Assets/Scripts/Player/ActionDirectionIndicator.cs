using UnityEngine;

public class ActionDirectionIndicator : MonoBehaviour
{
    [SerializeField] private Color _attackColor = Color.red;
    [SerializeField] private Color _healColor = Color.green;
    private SpriteRenderer _renderer;
    private Character _targeter;
    private Character _target;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        RefreshIsActive();
    }

    public void SetTargeter(Character targeter)
    {
        _targeter = targeter;
        RefreshIsActive();
        RefreshColor();
    }

    public void SetTarget(Character target)
    {
        _target = target;
        if (target)
        {
            transform.position = target.transform.position;
        }
        RefreshIsActive();
        RefreshColor();
    }

    private void RefreshIsActive()
    {
        gameObject.SetActive(ShouldBeActive());
    }

    private bool ShouldBeActive()
    {
        return _target && _targeter && _target != _targeter;
    }

    private void RefreshColor()
    {
        _renderer.color = CalculateCorrectColor();
    }

    private Color CalculateCorrectColor()
    {
        bool bothExist = _target && _targeter;
        return bothExist ? 
            (_targeter.Faction == _target.Faction ? _healColor : _attackColor) : 
            Color.white;
    }

    private void Update()
    {
        Vector2 toTarget = _target.CurrentCellCenter - _targeter.CurrentCellCenter;
        transform.up = toTarget;
    }
}