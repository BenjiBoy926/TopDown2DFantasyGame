using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BattleCameraGrab))]
public class BattleCamera : MonoBehaviour
{
    public bool IsGrabbed => _grab.IsActive;
    private float WorldHeight => _camera.orthographicSize * 2;
    private float WorldWidth => WorldHeight * _camera.aspect;
    public Vector2 WorldSize => new(WorldWidth, WorldHeight);
    private Vector2 WorldExtents => WorldSize / 2f;
    private float CurrentZoom => OrthoSizeToZoom(_camera.orthographicSize);
    public Vector2 Position
    {
        get => _rigidbody.position;
        set => _rigidbody.position = value;
    }
    public Vector2 Velocity
    {
        get => _rigidbody.velocity;
        set => _rigidbody.velocity = value;
    }

    [SerializeField] private float _viewMargin = 1;
    [SerializeField] private Vector2 _viewSizeRange = new(5, 15);

    [Space]
    [SerializeField] private float _zoomJump = 0.3f;
    [SerializeField] private float _zoomJumpDuration = 0.35f;
    [SerializeField] private Ease _zoomJumpEase = Ease.OutQuint;

    private Camera _camera;
    private Rigidbody2D _rigidbody;
    private BattleCameraGrab _grab;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _grab = GetComponent<BattleCameraGrab>();
    }

    public void Grab(Vector2 worldPosition)
    {
        _grab.Begin(worldPosition);
    }

    public void UpdateGrab(Vector2 screenPosition)
    {
        _grab.UpdateGrab(screenPosition);
    }

    public void Release()
    {
        _grab.End();
    }

    public Vector2 ScreenToWorld(Vector2 screen)
    {
        return _camera.ScreenToWorldPoint(screen);
    }

    public Vector2 WorldToScreen(Vector2 world)
    {
        return _camera.WorldToScreenPoint(world);
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