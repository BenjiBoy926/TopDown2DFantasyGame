using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RangeWarning : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 0.2f;
    private Character _attacker;
    private Character _target;
    private LineRenderer _line;
    private readonly Vector3[] _positions = new Vector3[]
    {
        Vector3.zero, Vector3.zero
    };
    private bool _isVisible = false;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }

    public void Begin(Character attacker, Character target)
    {
        _attacker = attacker;
        _target = target;
        _positions[0] = _attacker.transform.position;
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
        _positions[1] = _target.transform.position;
        _line.SetPositions(_positions);
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
        FadeAlpha(1);
    }

    private void FadeOut()
    {
        FadeAlpha(0);
    }

    private void FadeAlpha(float alpha)
    {
        Color color = Color.red;
        color.a = alpha;
        Color2 value = new(color, color);
        _line.DOColor(value, value, _fadeDuration);
    }
}