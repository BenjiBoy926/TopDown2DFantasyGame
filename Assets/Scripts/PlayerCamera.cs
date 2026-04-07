using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCamera : MonoBehaviour
{
    public bool IsGrabbed => _isGrabbed;

    private Camera _camera;
    private Rigidbody2D _rigidbody;

    private Vector2 _grabScreenPosition;
    private Vector2 _grabCameraPosition;
    private Vector2 _previousCameraPosition;
    private float _timeOfLastCameraUpdate;
    private bool _isGrabbed;

    public void Grab(Vector2 worldPosition)
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(worldPosition);
        _grabScreenPosition = screenPosition;
        _grabCameraPosition = _rigidbody.position;
        _previousCameraPosition = _rigidbody.position;
        _timeOfLastCameraUpdate = Time.time;
        _isGrabbed = true;
    }

    public void UpdateFromScreenPosition(Vector2 screenPosition)
    {
        Vector2 screenOffset = screenPosition - _grabScreenPosition;
        Vector2 screenSize = new(Screen.width, Screen.height);
        Vector2 normalizedOffset = -(screenOffset / screenSize);

        float cameraWorldHeight = _camera.orthographicSize * 2;
        float cameraWorldWidth = cameraWorldHeight * _camera.aspect;
        Vector2 cameraWorldSize = new(cameraWorldWidth, cameraWorldHeight);

        Vector2 worldOffset = normalizedOffset * cameraWorldSize;
        _previousCameraPosition = _rigidbody.position;
        _timeOfLastCameraUpdate = Time.time;
        _rigidbody.position = _grabCameraPosition + worldOffset;
    }

    public void Release()
    {
        if (!_isGrabbed) return;

        _isGrabbed = false;
        Debug.Log("Set velocity from updates");
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