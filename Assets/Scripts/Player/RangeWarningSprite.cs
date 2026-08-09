using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RangeWarningSprite : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = .2f;
    private SpriteRenderer _renderer;
    private float _originalAlpha;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _originalAlpha = _renderer.color.a;
    }

    private void OnDestroy()
    {
        _renderer.DOKill();
    }

    public void SetAlpha(float alpha)
    {
        Color color = _renderer.color;
        color.a = alpha;
        _renderer.color = color;
    }

    public void FadeIn()
    {
        _renderer.DOKill();
        _renderer.DOFade(_originalAlpha, _fadeDuration);
    }

    public void FadeOut()
    {
        _renderer.DOKill();
        _renderer.DOFade(0, _fadeDuration);
    }
}