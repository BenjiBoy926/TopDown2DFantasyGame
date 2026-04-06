using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCamera : MonoBehaviour
{
    private Camera _camera;
    private Rigidbody2D _rigidbody;
    private Vector2 _grabScreenPosition;
    private Vector2 _grabCameraPosition;

    public void Grab(Vector2 worldPosition)
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(worldPosition);
        _grabScreenPosition = screenPosition;
        _grabCameraPosition = _rigidbody.position;
    }

    public void UpdateFromScreenPosition(Vector2 screenPosition)
    {
        Vector2 offset = screenPosition - _grabScreenPosition;
        Matrix4x4 projection = _camera.projectionMatrix;
        Debug.Log($"Screen offset: {offset}");
        // have to convert that screen offset to a world offset using... the projection matrix?  The ortho size?
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