using DG.Tweening;
using Hellmade.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionTimerUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _iconCircle;
    [SerializeField] private Image _outline;
    [Space]
    [SerializeField] private float _shownAlpha = .5f;
    [SerializeField] private float _fadeDuration = .2f;
    [SerializeField] private float _confirmDuration = .35f;
    [Space]
    [SerializeField] private AudioSource _loadingSource;
    [SerializeField] private AudioClip _confirmSound;
    private TMP_Text _label;
    private bool _isActive;

    private void Awake()
    {
        _label = GetComponentInChildren<TMP_Text>(true);
        _iconCircle.transform.localScale = Vector3.zero;        
        _iconCircle.color = new(_iconCircle.color.r, _iconCircle.color.g, _iconCircle.color.b, 0f);
        _outline.color = new(_outline.color.r, _outline.color.g, _outline.color.b, 0f);
        _label.color = new(_label.color.r, _label.color.g, _label.color.b, 0f);
    }

    public void Begin(PlayerTimedAction action, float duration)
    {
        _icon.sprite = action.Sprite;
        _label.text = action.Label;
        Animate(duration);
    }

    private void Animate(float duration)
    {
        KillAllTweens();

        _iconCircle.transform.localScale = Vector3.zero;
        _iconCircle.transform.DOScale(1, duration).SetEase(Ease.OutQuint);
        _outline.transform.localScale = Vector3.one;

        Fade(_shownAlpha, duration);

        _isActive = true;
        _loadingSource.Play();
    }

    public void CancelAnimation()
    {
        if (!_isActive)
            return;

        KillAllTweens();

        _iconCircle.transform.DOScale(0, _fadeDuration);

        Fade(0, _fadeDuration);

        _isActive = false;
        _loadingSource.Stop();
    }

    public void PlayConfirmAnimation()
    {
        KillAllTweens();

        _iconCircle.transform.DOScale(0, _confirmDuration).SetEase(Ease.InBack);
        _outline.transform.DOScale(2, _confirmDuration).SetEase(Ease.OutQuad);

        Fade(0, _confirmDuration);
        
        _isActive = false;
        EazySoundManager.PlayUISound(_confirmSound);
        _loadingSource.Stop();
    }

    private void Fade(float alpha, float duration)
    {
        _iconCircle.DOFade(alpha, duration);
        _outline.DOFade(alpha, duration);
        _label.DOFade(alpha, duration);
    }

    private void KillAllTweens()
    {
        _iconCircle.transform.DOKill();
        _outline.transform.DOKill();

        _iconCircle.DOKill();
        _outline.DOKill();
        _label.DOKill();
    }
}