using DG.Tweening;
using UnityEngine;

public class PlayerGridReticle : MonoBehaviour
{
    [SerializeField] private float _moveDuration = .2f;
    [SerializeField] private Ease _moveEase = Ease.OutQuad;

    [Space]
    [SerializeField] private float _breathDuration = 1f;
    [SerializeField] private Vector2 _breathScaleRange = new(.8f, 1f);

    private Tween _moveTween;

    private void Awake()
    {
        transform.localScale = Vector2.one * _breathScaleRange.x;
        transform.DOScale(_breathScaleRange.y, _breathDuration * .5f)
            .SetEase(Ease.OutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void MoveToPosition(Vector2 position)
    {
        _moveTween?.Kill();
        _moveTween = transform.DOMove(position, _moveDuration).SetEase(_moveEase);
    }
}