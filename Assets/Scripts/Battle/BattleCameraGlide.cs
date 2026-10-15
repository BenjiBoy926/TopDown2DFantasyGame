using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(BattleCamera))]
public class BattleCameraGlide : MonoBehaviour
{
    public bool IsActive => _coroutine != null;

    [SerializeField] private float _duration = 0.35f;
    [SerializeField] private AnimationCurve _ease;
    private Coroutine _coroutine;
    private BattleCamera _camera;

    private void Awake()
    {
        _camera = GetComponent<BattleCamera>();
    }

    public Coroutine Begin(Transform target)
    {
        return Begin(GetGlideSequence(target));
    }

    public Coroutine Begin(Vector3 position)
    {
        return Begin(GetGlideSequence(position));
    }

    public void End()
    {
        StopAllCoroutines();
        _coroutine = null;
    }

    private Coroutine Begin(IEnumerator sequence)
    {
        StopAllCoroutines();
        _coroutine = StartCoroutine(sequence);
        return _coroutine;
    }

    private IEnumerator GetGlideSequence(Vector3 position)
    {
        return GetGlideSequence(() => position);
    }

    private IEnumerator GetGlideSequence(Transform target)
    {
        return GetGlideSequence(() => target.position);
    }

    private IEnumerator GetGlideSequence(Func<Vector3> getTargetPosition)
    {
        Vector3 startPosition = transform.position;

        float startTime = Time.time;
        float elapsedTime = Time.time - startTime;

        while (elapsedTime < _duration)
        {
            float progress = elapsedTime / _duration;
            Vector3 endPosition = _camera.GetFramingPosition(getTargetPosition());
            float t = _ease.Evaluate(progress);
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, t);
            transform.position = currentPosition;

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        transform.position = _camera.GetFramingPosition(getTargetPosition());
        End();
    }
}