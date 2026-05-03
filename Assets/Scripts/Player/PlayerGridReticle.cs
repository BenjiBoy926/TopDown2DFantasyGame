using DG.Tweening;
using UnityEngine;

public class PlayerGridReticle : MonoBehaviour
{
    [SerializeField] private float _moveDuration = .2f;
    [SerializeField] private Ease _moveEase = Ease.OutQuad;

    public void MoveToPosition(Vector2 position)
    {
        transform.DOKill();
        transform.DOMove(position, _moveDuration).SetEase(_moveEase);
    }
}