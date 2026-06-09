using DG.Tweening;
using UnityEngine;

public class PlayerSelectionAura : MonoBehaviour
{
    [SerializeField] private Ease _showEase = Ease.OutQuad;
    [SerializeField] private Ease _hideEase = Ease.InBack;
    [SerializeField] private float _showHideDuration = .35f;
    private Tween _showHideTween;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    public void Show()
    {
        _showHideTween?.Kill();
        _showHideTween = transform.DOScale(1, _showHideDuration).SetEase(_showEase);
    }

    public void Hide()
    {
        _showHideTween?.Kill();
        _showHideTween = transform.DOScale(0, _showHideDuration).SetEase(_hideEase);
    }
}