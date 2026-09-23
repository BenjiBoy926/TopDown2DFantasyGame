using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
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

    [SerializeField] private Image _icon;
    [SerializeField] private Image _outline;
    [SerializeField] private DisplayInfo _undoInfo;
    [SerializeField] private DisplayInfo _redoInfo;
    [SerializeField] private float _shownAlpha = .5f;
    [SerializeField] private float _fadeDuration = .2f;
    [SerializeField] private float _confirmDuration = .35f;
    private TMP_Text _label;

    private void Awake()
    {
        _label = GetComponentInChildren<TMP_Text>(true);
        _icon.color = new(_icon.color.r, _icon.color.g, _icon.color.b, 0f);
        _outline.color = new(_outline.color.r, _outline.color.g, _outline.color.b, 0f);
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
        _icon.sprite = info.Sprite;
        _label.text = info.Label;
    }

    private void Begin(float duration)
    {
        KillAllTweens();

        _icon.transform.localScale = Vector3.zero;
        _icon.transform.DOScale(1, duration).SetEase(Ease.OutQuint);
        _outline.transform.localScale = Vector3.one;

        Fade(_shownAlpha, duration);
    }

    public void Cancel()
    {
        KillAllTweens();

        _icon.transform.DOScale(0, _fadeDuration);

        Fade(0, _fadeDuration);
    }

    public void Confirm()
    {
        KillAllTweens();

        _icon.transform.DOScale(0, _confirmDuration).SetEase(Ease.InBack);
        _outline.transform.DOScale(2, _confirmDuration).SetEase(Ease.OutQuad);

        Fade(0, _confirmDuration);
    }

    private void Fade(float alpha, float duration)
    {
        _icon.DOFade(alpha, duration);
        _outline.DOFade(alpha, duration);
        _label.DOFade(alpha, duration);
    }

    private void KillAllTweens()
    {
        _icon.transform.DOKill();
        _outline.transform.DOKill();

        _icon.DOKill();
        _outline.DOKill();
        _label.DOKill();
    }
}