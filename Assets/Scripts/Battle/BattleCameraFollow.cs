using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BattleCamera))]
public class BattleCameraFollow : MonoBehaviour
{
    private Transform _target;
    private BattleCamera _camera;

    private void Awake()
    {
        _camera = GetComponent<BattleCamera>();
    }

    public Coroutine Begin(Transform target)
    {
        _target = target;
        return _camera.Glide(target);
    }

    private void Update()
    {
        if (_target && !_camera.IsGliding)
        {
            transform.position = _camera.GetFramingPosition(_target);
        }
    }

    public void End()
    {
        _target = null;
        _camera.EndGlide();
    }
}