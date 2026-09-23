using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionTimerUI : MonoBehaviour
{
    [Serializable]
    private struct DisplayInfo
    {
        public Sprite Sprite;
        public string Label;
    }

    [SerializeField] private DisplayInfo _undoInfo;
    [SerializeField] private DisplayInfo _redoInfo;
    [SerializeField] private float _shownAlpha = .5f;
    [SerializeField] private float _fadeDuration = .2f;
    [SerializeField] private float _confirmDuration = .35f;
    private Image _image;
    private TMP_Text _label;

    private void Awake()
    {
        _image = GetComponentInChildren<Image>(true);
        _label = GetComponentInChildren<TMP_Text>(true);
        _image.color = new(_image.color.r, _image.color.g, _image.color.b, 0f);
        _label.color = new(_label.color.r, _label.color.g, _label.color.b, 0f);
    }

    public void BeginUndo(float duration)
    {
        Begin(duration);
        Display(_undoInfo);
    }

    public void BeginRedo(float duration)
    {
        Begin(duration);
        Display(_redoInfo);
    }

    private void Display(DisplayInfo info)
    {
        _image.sprite = info.Sprite;
        _label.text = info.Label;
    }

    private void Begin(float duration)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, duration).SetEase(Ease.OutQuint);

        _image.DOFade(_shownAlpha, _fadeDuration);
        _label.DOFade(_shownAlpha, _fadeDuration);
    }

    public void Cancel()
    {
        transform.DOKill();
        transform.DOScale(0, _fadeDuration);

        _image.DOFade(0f, _fadeDuration);
        _label.DOFade(0f, _fadeDuration);
    }

    public void Confirm()
    {
        transform.DOKill();
        transform.DOScale(0, _confirmDuration).SetEase(Ease.InBack);
    }
}