using DG.Tweening;
using System.Collections;
using UnityEngine;
using static CharacterAnimator;

[RequireComponent(typeof(LineRenderer))]
public class ArrowBlur : MonoBehaviour
{
    [SerializeField] private float _duration = .1f;
    [SerializeField] private float _width = .5f;
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
        Vector2 targetPoint = target.Position + new Vector2(0, _sidePosition.y);
        _line.SetPosition(0, arrowLaunchPoint);
        _line.SetPosition(1, targetPoint);

        _line.enabled = true;
        _line.widthMultiplier = _width;
        DOTween.To(GetLineWidthMultiplier, SetLineWidthMultiplier, 0, _duration).OnComplete(OnWidthTweenComplete);
    }

    private Vector2 GetArrowLaunchPoint(Character target)
    {
        Vector2 offset = GetArrowOffset(target);
        return (Vector2)transform.position + offset;
    }

    private Vector2 GetArrowOffset(Character target)
    {
        Vector2 direction = target.Position - _character.Position;

        bool isHorizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
        bool isVertical = !isHorizontal;

        if (isHorizontal && direction.x > 0)
        {
            return _sidePosition;
        }
        if (isHorizontal && direction.x < 0)
        {
            return new(-_sidePosition.x, _sidePosition.y);
        }
        if (isVertical && direction.y > 0)
        {
            return _upPosition;
        }
        return _downPosition;
    }

    private float GetLineWidthMultiplier()
    {
        return _line.widthMultiplier;
    }

    private void SetLineWidthMultiplier(float width)
    {
        _line.widthMultiplier = width;
    }

    private void OnWidthTweenComplete()
    {
        _line.enabled = false;
    }
}