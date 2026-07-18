using UnityEngine;

[RequireComponent(typeof(BattleCamera))]
public class BattleCameraGrab : MonoBehaviour
{
    public bool IsActive => _isActive;

    private BattleCamera _camera;
    private Vector2 _grabScreenPosition;
    private Vector2 _grabWorldPosition;
    private Vector2 _previousWorldPosition;
    private float _previousUpdateTime;
    private Vector2 _currentWorldPosition;
    private float _currentUpdateTime;
    private bool _isActive;

    private void Awake()
    {
        _camera = GetComponent<BattleCamera>();
    }

    public void Begin(Vector2 worldPosition)
    {
        Vector2 screenPosition = _camera.WorldToScreen(worldPosition);

        _grabScreenPosition = screenPosition;
        _grabWorldPosition = _camera.Position;

        _previousWorldPosition = _grabWorldPosition;
        _previousUpdateTime = Time.time;

        _currentWorldPosition = _grabWorldPosition;
        _currentUpdateTime = Time.time;

        _isActive = true;

        _camera.Velocity = Vector2.zero;
    }

    public void UpdateGrab(Vector2 screenPosition)
    {
        Vector2 screenOffset = screenPosition - _grabScreenPosition;
        Vector2 screenSize = new(Screen.width, Screen.height);
        Vector2 normalizedOffset = -(screenOffset / screenSize);
        Vector2 worldOffset = normalizedOffset * _camera.WorldSize;

        _previousWorldPosition = _currentWorldPosition;
        _previousUpdateTime = _currentUpdateTime;

        _currentWorldPosition = _grabWorldPosition + worldOffset;
        _currentUpdateTime = Time.time;

        _camera.Position = _currentWorldPosition;
    }

    public void End()
    {
        if (!_isActive) return;

        _isActive = false;

        Vector2 dx = _currentWorldPosition - _previousWorldPosition;
        float dt = _currentUpdateTime - _previousUpdateTime;
        if (dt > 0)
        {
            _camera.Velocity = dx / dt;
        }
    }
}