using DG.Tweening;
using UnityEngine;

public class PlayerGridReticle : MonoBehaviour
{
    [SerializeField] private float _moveDuration = .2f;
    [SerializeField] private Ease _moveEase = Ease.OutQuad;

    private Tween _moveTween;

    public void MoveToPosition(Vector2 position)
    {
        _moveTween?.Kill();
        _moveTween = transform.DOMove(position, _moveDuration).SetEase(_moveEase);
    }
}