using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CharacterSpeechBubblePointer : MonoBehaviour
{
    [Tooltip("Shrink the bubble rect by this much to prevent a point right on the rect edge counting on 'inside'")]
    [SerializeField] private int _rectPadding = 2;
    [SerializeField] private int _insideRoundedEdgesOffset = 20;
    [SerializeField] private float _diagonalOffset = 15;
    private RectTransform _rect;
    private RectTransform _parentRect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _parentRect = transform.parent.GetComponent<RectTransform>();
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
        _rect.anchoredPosition = new(
            Mathf.Clamp(screenPoint.x, insideRoundedEdges.xMin, insideRoundedEdges.xMax),
            Mathf.Clamp(screenPoint.y, insideRoundedEdges.yMin, insideRoundedEdges.yMax));

        int quadrant = GetOffsetQuadrant(screenPoint, bubbleRect);
        float angle = quadrant * 90;
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

        int quadrant = GetOffsetQuadrant(screenPoint, bubbleRect);
        float angle = quadrant * 90 + 45;
        _rect.localEulerAngles = new(0, 0, angle);
    }

    private static int GetOffsetQuadrant(Vector2 screenPoint, Rect bubbleRect)
    {
        Vector2 toTarget = screenPoint - bubbleRect.center;
        float atan = Mathf.Atan2(toTarget.y, toTarget.x);
        const float piOver2 = Mathf.PI / 2f;
        int quadrant = Mathf.FloorToInt(atan / piOver2);
        return quadrant;
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