using DG.Tweening;
using UnityEngine;


public class BattleCameraFollow : MonoBehaviour
{
    [SerializeField] private float _initialFollowDuration = 0.35f;
    [SerializeField] private Ease _initialFollowEase = Ease.OutQuad;
    private Transform _target;
    private Tween _initialFollowTween;

    public void Begin(Transform target)
    {
        Vector3 targetPosition = GetCameraPositionFromFollowTarget(target);
        _target = target;
        _initialFollowTween = transform.DOMove(targetPosition, _initialFollowDuration).SetEase(_initialFollowEase);
    }

    private void Update()
    {
        if (_initialFollowTween.IsActive() || !_target)
        {
            return;
        }
        transform.position = GetCameraPositionFromFollowTarget(_target);
    }

    public void End()
    {
        _target = null;
        _initialFollowTween.Kill();
    }

    private Vector3 GetCameraPositionFromFollowTarget(Transform target)
    {
        Vector2 targetPosition = target.position;
        float z = transform.position.z;
        return new(targetPosition.x, targetPosition.y, z);
    }
}