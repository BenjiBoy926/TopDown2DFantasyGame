using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BattleCameraFollow : MonoBehaviour
{
    [SerializeField] private float _initialFollowDuration = 0.35f;
    [SerializeField] private AnimationCurve _initialFollowEase;
    private Transform _target;
    private Coroutine _initialFollowRoutine;

    public void Begin(Transform target)
    {
        _target = target;
        StopAllCoroutines();
        _initialFollowRoutine = StartCoroutine(GetInitialFollowRoutine());
    }

    private void Update()
    {
        if (!_target)
            return;

        if (_target && _initialFollowRoutine == null)
        {
            transform.position = GetTargetPosition();
        }
    }

    public void End()
    {
        _target = null;
        StopAllCoroutines();
        _initialFollowRoutine = null;
    }

    private IEnumerator GetInitialFollowRoutine()
    {
        Vector3 startPosition = transform.position;

        float startTime = Time.time;
        float elapsedTime = Time.time - startTime;

        while (elapsedTime < _initialFollowDuration)
        {
            float progress = elapsedTime / _initialFollowDuration;
            Vector3 endPosition = GetTargetPosition();
            float t = _initialFollowEase.Evaluate(progress);
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, t);
            transform.position = currentPosition;

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        transform.position = GetTargetPosition();
        _initialFollowRoutine = null;
    }

    private Vector3 GetTargetPosition()
    {
        Vector2 targetPosition = _target.position;
        float z = transform.position.z;
        return new(targetPosition.x, targetPosition.y, z);
    }
}