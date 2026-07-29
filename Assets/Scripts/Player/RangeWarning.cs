using UnityEngine;

public class RangeWarning : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 0.2f;
    private Character _attacker;
    private Character _target;
    private bool _isVisible = false;

    public void Begin(Character attacker, Character target)
    {
        _attacker = attacker;
        _target = target;
        Refresh();
    }

    public void Refresh()
    {
        _attacker.RefreshRange();
        SetIsVisible(_attacker.IsReachable(_target.CurrentCell));
    }

    public void End()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        Vector2 attackerPosition = _attacker.Position;
        Vector2 targetPosition = _target.Position;
        float distance = Vector2.Distance(attackerPosition, targetPosition);

        transform.position = attackerPosition;
        transform.up = targetPosition - attackerPosition;
        transform.localScale = new(1, distance, 1);
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
        gameObject.SetActive(true);
    }

    private void FadeOut()
    {
        gameObject.SetActive(false);
    }
}