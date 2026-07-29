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
        Vector3 attackerPosition = _attacker.Position;
        Vector3 targetPosition = _target.Position;
        transform.position = (attackerPosition + targetPosition) / 2;
        transform.up = targetPosition - attackerPosition;
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