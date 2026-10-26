using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(RectTransform))]
public class CharacterSpeechBubblePointer : MonoBehaviour
{
    [Tooltip("Shrink the bubble rect by this much to prevent a point right on the rect edge counting on 'inside'")]
    [SerializeField] private int _rectPadding = 2;
    [SerializeField] private int _interiorBackgroundOffset = 20;
    [SerializeField] private float _diagonalOffset = 15;
    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void PointTo(Vector2 screenPoint, Rect bubbleRect)
    {
        RectOffset offset = new(_rectPadding, _rectPadding, _rectPadding, _rectPadding);
        bubbleRect = offset.Remove(bubbleRect);
        bool show = !bubbleRect.Contains(screenPoint);

        gameObject.SetActive(show);
        if (show)
        {
            RefreshDirection(screenPoint, bubbleRect);
        }
    }

    private void RefreshDirection(Vector2 screenPoint, Rect bubbleRect)
    {
        Rect backgroundRect = GetBackgroundRect(bubbleRect);
        if (IsInHorizontalOrVerticalRange(screenPoint, backgroundRect))
        {
            PointCardinally(screenPoint, bubbleRect);
        }
        else
        {
            PointDiagonally(screenPoint, bubbleRect);
        }
    }

    private void PointCardinally(Vector2 screenPoint, Rect bubbleRect)
    {
        Rect backgroundRect = GetBackgroundRect(bubbleRect);
        _rect.anchorMin = _rect.anchorMax = Vector2.zero;
        if (IsInRange(screenPoint, backgroundRect, 0))
        {
            _rect.anchoredPosition = new(screenPoint.x, screenPoint.y < backgroundRect.yMin ? 0 : bubbleRect.height);
        }
        else
        {
            _rect.anchoredPosition = new(screenPoint.x < backgroundRect.xMin ? 0 : bubbleRect.width, screenPoint.y);
        }

        float angle = 0;
        if (screenPoint.y < backgroundRect.yMin)
        {
            angle = 180;
        }
        else if (screenPoint.x < backgroundRect.xMin)
        {
            angle = 90;
        }
        else if (screenPoint.x > backgroundRect.xMax)
        {
            angle = -90;
        }
        _rect.localEulerAngles = new(0, 0, angle);
    }

    private bool IsInHorizontalOrVerticalRange(Vector2 point, Rect rect)
    {
        return IsInRange(point, rect, 0) || IsInRange(point, rect, 1);
    }

    private bool IsInRange(Vector2 point, Rect rect, int dimension)
    {
        return point[dimension] >= rect.min[dimension] && point[dimension] <= rect.max[dimension];
    }

    private void PointDiagonally(Vector2 screenPoint, Rect bubbleRect)
    {
        Vector2 toTarget = screenPoint - bubbleRect.center;
        Vector2 toTargetSign = new(Mathf.Sign(toTarget.x), Mathf.Sign(toTarget.y));
        _rect.anchorMin = _rect.anchorMax = CalculateAnchor(toTarget);
        _rect.anchoredPosition = new(_diagonalOffset * toTargetSign.x * -1, _diagonalOffset * toTargetSign.y * -1);

        float angle;
        if (toTarget.x < 0 && toTarget.y > 0)
        {
            angle = (0 * 90) + 45;
        }
        else if (toTarget.x < 0 && toTarget.y < 0)
        {
            angle = (1 * 90) + 45;
        }
        else if (toTarget.x > 0 && toTarget.y < 0)
        {
            angle = (2 * 90) + 45;
        }
        else
        {
            angle = (3 * 90) + 45;
        }
        _rect.localEulerAngles = new(0, 0, angle);
    }

    private Rect GetBackgroundRect(Rect bubbleRect)
    {
        RectOffset offset = new(_interiorBackgroundOffset, _interiorBackgroundOffset, _interiorBackgroundOffset, _interiorBackgroundOffset);
        Rect backgroundRect = offset.Remove(bubbleRect);
        return backgroundRect;
    }

    private Vector2 CalculateAnchor(Vector2 direction)
    {
        return new(CalculateAnchor(direction, 0), CalculateAnchor(direction, 1));
    }

    private float CalculateAnchor(Vector2 direction, int dimension)
    {
        return direction[dimension] < 0 ? 0 : 1;
    }
}