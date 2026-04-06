using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCamera : MonoBehaviour
{
    private Camera _camera;
    private Rigidbody2D _rigidbody;

    public void Grab(Transform source)
    {
        Debug.Log("Camera grabbed");
    }

    public void Release()
    {
        Debug.Log("Camera released");
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
}