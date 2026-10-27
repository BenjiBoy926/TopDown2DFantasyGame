using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CharacterSpeechBubblePointer : MonoBehaviour
{
    [Tooltip("Shrink the bubble rect by this much to prevent a point right on the rect edge counting on 'inside'")]
    [SerializeField] private int _rectPadding = 2;
    [SerializeField] private int _insideRoundedEdgesOffset = 20;
    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void PointTo(Vector2 screenPoint, Rect bubbleRect)
    {
        RectOffset offset = new(_rectPadding, _rectPadding, _rectPadding, _rectPadding);
        Rect containsRect = offset.Remove(bubbleRect);
        bool show = !containsRect.Contains(screenPoint);

        gameObject.SetActive(show);
        if (show)
        {
            RefreshDirection(screenPoint, bubbleRect);
        }
    }

    private void RefreshDirection(Vector2 screenPoint, Rect bubbleRect)
    {
        Rect insideRoundedEdges = GetRectInsideRoundedEdges(bubbleRect);
        if (IsInHorizontalOrVerticalRange(screenPoint, insideRoundedEdges))
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
        Rect insideRoundedEdges = GetRectInsideRoundedEdges(bubbleRect);
        _rect.anchorMin = _rect.anchorMax = Vector2.zero;
        Vector2 positionOnScreen = new(
            Mathf.Clamp(screenPoint.x, insideRoundedEdges.xMin, insideRoundedEdges.xMax),
            Mathf.Clamp(screenPoint.y, insideRoundedEdges.yMin, insideRoundedEdges.yMax));
        _rect.anchoredPosition = positionOnScreen - insideRoundedEdges.min;

        float quadrant = GetOffsetQuadrant(screenPoint, bubbleRect);
        float quadrandIndex = Mathf.Round(quadrant);
        float angle = quadrandIndex * 90;
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
        _rect.anchorMin = _rect.anchorMax = CalculateAnchor(toTarget);
        _rect.anchoredPosition = Vector2.zero;

        float quadrant = GetOffsetQuadrant(screenPoint, bubbleRect);
        float quadrantIndex = Mathf.Floor(quadrant);
        float angle = quadrantIndex * 90 + 45;
        _rect.localEulerAngles = new(0, 0, angle);
    }

    private static float GetOffsetQuadrant(Vector2 screenPoint, Rect bubbleRect)
    {
        Vector2 toTarget = screenPoint - bubbleRect.center;
        float atan = Mathf.Atan2(toTarget.y, toTarget.x);
        const float piOver2 = Mathf.PI / 2f;
        return atan / piOver2;
    }

    private Rect GetRectInsideRoundedEdges(Rect bubbleRect)
    {
        RectOffset offset = new(_insideRoundedEdgesOffset, _insideRoundedEdgesOffset, _insideRoundedEdgesOffset, _insideRoundedEdgesOffset);
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