using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CharacterSpeechBubblePointer : MonoBehaviour
{
    [Tooltip("Shrink the bubble rect by this much to prevent a point right on the rect edge counting on 'inside'")]
    [SerializeField] private int _rectPadding = 2;
    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void PointTo(Vector2 screenPoint, Rect bubbleRect)
    {
        RectOffset offset = new(_rectPadding, _rectPadding, _rectPadding, _rectPadding);
        bubbleRect = offset.Remove(bubbleRect);

        gameObject.SetActive(!bubbleRect.Contains(screenPoint));

        Vector2 toTarget = screenPoint - bubbleRect.center;
        _rect.anchorMin = _rect.anchorMax = CalculateAnchor(toTarget);
        _rect.anchoredPosition = Vector2.zero;

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

    private Vector2 CalculateAnchor(Vector2 direction)
    {
        return new(CalculateAnchor(direction, 0), CalculateAnchor(direction, 1));
    }

    private float CalculateAnchor(Vector2 direction, int dimension)
    {
        return direction[dimension] < 0 ? 0 : 1;
    }
}