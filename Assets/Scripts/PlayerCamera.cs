using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCamera : MonoBehaviour
{
    private Camera _camera;
    private Rigidbody2D _rigidbody;

    public void Slide(Vector2 delta)
    {
        _rigidbody.position += delta;
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
}