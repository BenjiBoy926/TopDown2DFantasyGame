using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArrowBlur : MonoBehaviour
{
    [SerializeField] private float _duration = .1f;
    [SerializeField] private Vector2 _upPosition = new(0, .6f);
    [SerializeField] private Vector2 _sidePosition = new(.4f, .2f);
    [SerializeField] private Vector2 _downPosition = new(0, -.1f);
    private LineRenderer _line;
    private Character _character;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _character = GetComponentInParent<Character>();
    }

    public void Play(Character target)
    {
        Vector2 arrowLaunchPoint = GetArrowLaunchPoint(target);
        transform.localPosition = arrowLaunchPoint;
    }

    private Vector2 GetArrowLaunchPoint(Character target)
    {
        return Vector2.zero;
    }
}