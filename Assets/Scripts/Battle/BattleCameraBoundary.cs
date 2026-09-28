using UnityEngine;

[RequireComponent(typeof(BattleCamera))]
public class BattleCameraBoundary : MonoBehaviour
{
    private Battle _battle;
    private BattleCamera _camera;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _camera = GetComponent<BattleCamera>();
    }

    private void Update()
    {
        Vector2 position = _camera.Position;
        Rect battleArea = _battle.Area;
        position.x = Mathf.Clamp(position.x, battleArea.xMin, battleArea.xMax);
        position.y = Mathf.Clamp(position.y, battleArea.yMin, battleArea.yMax);
        _camera.Position = position;
    }
}