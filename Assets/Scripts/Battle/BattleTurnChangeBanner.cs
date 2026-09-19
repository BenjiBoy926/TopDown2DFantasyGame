using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class BattleTurnChangeBanner : MonoBehaviour
{
    [SerializeField] private float _animationDuration = .35f;

    private RectTransform _rectTransform;
    private Image _image;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();

        _rectTransform.localScale = new(0, 1, 1);
    }

    public YieldInstruction AnimateIn(Faction faction)
    {
        Color color = faction.Color;
        color.a = _image.color.a;
        _image.color = color;

        _rectTransform.pivot = new(0, .5f);
        _rectTransform.localScale = new(0, 1, 1);
        return _rectTransform.DOScaleX(1, _animationDuration).WaitForCompletion();
    }

    public YieldInstruction AnimateOut()
    {
        _rectTransform.pivot = new(1, .5f);
        return _rectTransform.DOScaleX(0, _animationDuration).WaitForCompletion();
    }
}