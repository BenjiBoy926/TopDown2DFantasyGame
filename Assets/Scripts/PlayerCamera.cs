using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCamera : MonoBehaviour
{
    public bool IsGrabbed => _isGrabbed;
    private float WorldHeight => _camera.orthographicSize * 2;
    private float WorldWidth => WorldHeight * _camera.aspect;
    private Vector2 WorldSize => new(WorldWidth, WorldHeight);
    private Vector2 WorldExtent => WorldSize / 2f;
    private Vector2 WorldMin => _rigidbody.position - WorldExtent;
    private Vector2 WorldMax => _rigidbody.position + WorldExtent;
    private Rect WorldRect => Rect.MinMaxRect(WorldMin.x, WorldMin.y, WorldMax.x, WorldMax.y);

    private Camera _camera;
    private Rigidbody2D _rigidbody;
    private Battle _battle;

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

    public void IncludeCellInView(Vector2Int cell)
    {
        Vector2 cellPosition = _battle.CellToWorld(cell);
        Rect worldRect = WorldRect;
        if (worldRect.Contains(cellPosition))
        {
            Debug.Log("Cell is already in view");
            return;
        }
        Debug.Log("Cell is not in view, must move to include it in view");
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _battle = GetComponentInParent<Battle>();
    }
}