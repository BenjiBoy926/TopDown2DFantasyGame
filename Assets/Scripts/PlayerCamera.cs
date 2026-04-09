using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCamera : MonoBehaviour
{
    public bool IsGrabbed => _isGrabbed;
    private float WorldHeight => _camera.orthographicSize * 2;
    private float WorldWidth => WorldHeight * _camera.aspect;
    private Vector2 WorldSize => new(WorldWidth, WorldHeight);
    private Vector2 WorldExtents => WorldSize / 2f;
    private float CurrentZoom => OrthoSizeToZoom(_camera.orthographicSize);

    [SerializeField] private float _viewMargin = 1;
    [SerializeField] private Vector2 _viewSizeRange = new(5, 15);

    [Space]
    [SerializeField] private float _zoomJump = 0.3f;
    [SerializeField] private float _zoomJumpDuration = 0.35f;
    [SerializeField] private Ease _zoomJumpEase = Ease.OutQuint;

    private Camera _camera;
    private Rigidbody2D _rigidbody;

    private Vector2 _grabScreenPosition;
    private Vector2 _grabWorldPosition;
    private Vector2 _previousWorldPosition;
    private float _previousUpdateTime;
    private Vector2 _currentWorldPosition;
    private float _currentUpdateTime;
    private bool _isGrabbed;

    public void Grab(Vector2 worldPosition)
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(worldPosition);

        _grabScreenPosition = screenPosition;
        _grabWorldPosition = _rigidbody.position;

        _previousWorldPosition = _grabWorldPosition;
        _previousUpdateTime = Time.time;

        _currentWorldPosition = _grabWorldPosition;
        _currentUpdateTime = Time.time;

        _isGrabbed = true;

        _rigidbody.velocity = Vector2.zero;
    }

    public void GrabUpdate(Vector2 screenPosition)
    {
        Vector2 screenOffset = screenPosition - _grabScreenPosition;
        Vector2 screenSize = new(Screen.width, Screen.height);
        Vector2 normalizedOffset = -(screenOffset / screenSize);
        Vector2 worldOffset = normalizedOffset * WorldSize;

        _previousWorldPosition = _currentWorldPosition;
        _previousUpdateTime = _currentUpdateTime;

        _currentWorldPosition = _grabWorldPosition + worldOffset;
        _currentUpdateTime = Time.time;

        _rigidbody.position = _currentWorldPosition;
    }

    public void Release()
    {
        if (!_isGrabbed) return;

        _isGrabbed = false;

        Vector2 dx = _currentWorldPosition - _previousWorldPosition;
        float dt = _currentUpdateTime - _previousUpdateTime;
        if (dt > 0)
        {
            _rigidbody.velocity = dx / dt;
        }
    }

    public Vector2 ScreenToWorld(Vector2 screen)
    {
        return _camera.ScreenToWorldPoint(screen);
    }

    public void IncludeInView(Vector2 position)
    {
        Rect rect = GetWorldRect(_viewMargin);
        Vector2 offset = OffsetOutsideEdge(rect, position);
        _rigidbody.position += offset;
    }

    public void ChangeZoom(float zoomDelta) 
    { 
        SetZoom(CurrentZoom + zoomDelta);
    }

    public void SetZoom(float zoom)
    {
        _camera.orthographicSize = ZoomToOrthoSize(zoom);
    }

    public void ZoomIn()
    {
        ZoomJump(+_zoomJump);
    }

    public void ZoomOut()
    {
        ZoomJump(-_zoomJump);
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void ZoomJump(float jumpAmount)
    {
        float newZoom = CurrentZoom + jumpAmount;
        float newOrthoSize = ZoomToOrthoSize(newZoom);
        OrthoSizeJump(newOrthoSize);
    }

    private void OrthoSizeJump(float newOrthoSize)
    {
        if (Mathf.Approximately(_camera.orthographicSize, newOrthoSize))
        {
            return;
        }
        _camera.DOKill();
        _camera.DOOrthoSize(newOrthoSize, _zoomJumpDuration).SetEase(_zoomJumpEase);
    }

    private Rect GetWorldRect(float margins)
    {
        Vector2 marginVector = new(margins, margins);
        Vector2 extents = WorldExtents - (marginVector * 2);
        Vector2 center = _rigidbody.position;
        Vector2 min = center - extents;
        Vector2 max = center + extents;
        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    private static Vector2 OffsetOutsideEdge(Rect rect, Vector2 point)
    {
        float xOffset = OffsetOutsideRange(point.x, rect.xMin, rect.xMax);
        float yOffset = OffsetOutsideRange(point.y, rect.yMin, rect.yMax);
        return new(xOffset, yOffset);
    }

    private static float OffsetOutsideRange(float value, float min, float max)
    {
        if (value < min)
        {
            return value - min;
        }
        else if (value > max)
        {
            return value - max;
        }
        return 0;
    }

    private float OrthoSizeToZoom(float orthoSize)
    {
        float invZoom = Mathf.InverseLerp(_viewSizeRange.x, _viewSizeRange.y, orthoSize);
        return 1 - invZoom;
    }

    private float ZoomToOrthoSize(float zoom)
    {
        float t = 1 - zoom;
        return Mathf.Lerp(_viewSizeRange.x, _viewSizeRange.y, t);
    }
}