using DG.Tweening;
using UnityEngine;

public class BreathScaleTween : MonoBehaviour
{
    [SerializeField] private float _breathInScale = .8f;
    [SerializeField] private float _breathInDuration = .5f;
    [SerializeField] private Ease _breathInEase = Ease.OutQuad;

    [Space]
    [SerializeField] private float _breathOutScale = 1;
    [SerializeField] private float _breathOutDuration = 1f;
    [SerializeField] private Ease _breatOutEase = Ease.Linear;
    
    private Tween _current;

    private void Awake()
    {
        transform.localScale = Vector2.one * _breathOutScale;
        Play();
    }

    public void Play()
    {
        Stop();
        Tween breathInTween = transform.DOScale(_breathInScale, _breathInDuration).SetEase(_breathInEase);
        Tween breathOutTween = transform.DOScale(_breathOutScale, _breathOutDuration).SetEase(_breatOutEase);
        _current = DOTween.Sequence()
            .Append(breathInTween)
            .Append(breathOutTween)
            .SetLoops(-1, LoopType.Restart);
    }

    public void Stop()
    {
        _current?.Kill();
    }
}