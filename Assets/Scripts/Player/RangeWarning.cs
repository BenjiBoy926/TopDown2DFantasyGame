using DG.Tweening;
using UnityEngine;

public class RangeWarning : MonoBehaviour
{
    private Character _attacker;
    private Character _target;
    private Vector2Int _cellOfTarget;
    private bool _isVisible = false;
    private RangeWarningSprite[] _sprites;

    private void Awake()
    {
        _sprites = GetComponentsInChildren<RangeWarningSprite>();
    }

    public void Begin(Character attacker, Character target)
    {
        _attacker = attacker;
        _target = target;
        RefreshAttackerRange();
    }

    public void End()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!_target)
            return;

        SetCellOfTarget(_target.CurrentCell);

        Vector2 attackerPosition = _attacker.Position;
        Vector2 targetPosition = _target.Position;
        float distance = Vector2.Distance(attackerPosition, targetPosition);

        transform.position = attackerPosition;
        transform.up = targetPosition - attackerPosition;
        transform.localScale = new(1, distance, 1);
    }

    private void SetCellOfTarget(Vector2Int cell)
    {
        if (cell == _cellOfTarget)
            return;

        _cellOfTarget = cell;
        RefreshAttackerRange();
    }

    private void RefreshAttackerRange()
    {
        _attacker.RefreshRange();
        SetIsVisible(_attacker.IsReachable(_target.CurrentCell));
    }

    private void SetIsVisible(bool isVisible)
    {
        if (_isVisible == isVisible)
            return;

        _isVisible = isVisible;
        if (isVisible)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }

    private void FadeIn()
    {
        foreach (var sprite in _sprites)
        {
            sprite.FadeIn();
        }
    }

    private void FadeOut()
    {
        foreach (var sprite in _sprites)
        {
            sprite.FadeOut();
        }
    }

}