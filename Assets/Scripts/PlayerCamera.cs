using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCamera : MonoBehaviour
{
    public bool IsGrabbed => _isGrabbed;

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

        float cameraWorldHeight = _camera.orthographicSize * 2;
        float cameraWorldWidth = cameraWorldHeight * _camera.aspect;
        Vector2 cameraWorldSize = new(cameraWorldWidth, cameraWorldHeight);

        Vector2 worldOffset = normalizedOffset * cameraWorldSize;

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

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
}