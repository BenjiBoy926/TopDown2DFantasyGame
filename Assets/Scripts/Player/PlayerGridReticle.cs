using DG.Tweening;
using UnityEngine;

public class PlayerGridReticle : MonoBehaviour
{
    [SerializeField] private float _moveDuration = .2f;
    [SerializeField] private Ease _moveEase = Ease.OutQuad;

    [Space]
    [SerializeField] private float _breathInScale = 1;
    [SerializeField] private float _breathInDuration = .5f;
    [SerializeField] private Ease _breathInEase = Ease.OutQuad;
    [SerializeField] private float _breathOutScale = .8f;
    [SerializeField] private float _breathOutDuration = 1f;
    [SerializeField] private Ease _breatOutEase = Ease.Linear;

    private Tween _moveTween;

    private void Awake()
    {
        transform.localScale = Vector2.one * _breathOutScale;
        Tween breathInTween = transform.DOScale(_breathInScale, _breathInDuration).SetEase(_breathInEase);
        Tween breathOutTween = transform.DOScale(_breathOutScale, _breathOutDuration).SetEase(_breatOutEase);
        DOTween.Sequence()
            .Append(breathInTween)
            .Append(breathOutTween)
            .SetLoops(-1, LoopType.Restart);
    }

    public void MoveToPosition(Vector2 position)
    {
        _moveTween?.Kill();
        _moveTween = transform.DOMove(position, _moveDuration).SetEase(_moveEase);
    }
}