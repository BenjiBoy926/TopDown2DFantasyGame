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
        _camera.Position = _battle.ClampToField(_camera.Position);
    }
}