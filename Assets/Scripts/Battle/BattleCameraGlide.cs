using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BattleCamera))]
public class BattleCameraGlide : MonoBehaviour
{
    public bool IsActive => _coroutine != null;

    [SerializeField] private float _duration = 0.35f;
    [SerializeField] private AnimationCurve _ease;
    private Transform _target;
    private Coroutine _coroutine;
    private BattleCamera _camera;

    private void Awake()
    {
        _camera = GetComponent<BattleCamera>();
    }

    public void Begin(Transform target)
    {
        _target = target;
        StopAllCoroutines();
        _coroutine = StartCoroutine(GetInitialFollowRoutine());
    }

    public void End()
    {
        _target = null;
        StopAllCoroutines();
        _coroutine = null;
    }

    private IEnumerator GetInitialFollowRoutine()
    {
        Vector3 startPosition = transform.position;

        float startTime = Time.time;
        float elapsedTime = Time.time - startTime;

        while (elapsedTime < _duration)
        {
            float progress = elapsedTime / _duration;
            Vector3 endPosition = _camera.GetFramingPosition(_target);
            float t = _ease.Evaluate(progress);
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, t);
            transform.position = currentPosition;

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        transform.position = _camera.GetFramingPosition(_target);
        End();
    }
}