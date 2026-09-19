using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class BattleTurnChangeAnimation : MonoBehaviour
{
    private readonly static WaitForSeconds _waitForSeconds3 = new(3);

    public bool IsPlaying => _isPlaying;

    [SerializeField] private float _transitionDuration = .35f;
    [SerializeField] private float _holdDuration = 1.5f;
    [SerializeField] private float _overlayAlpha = .2f;
    [SerializeField] private float _bannerAlpha = .5f;

    private Image _overlay;
    private Image _banner;
    private TMP_Text _label;
    private bool _isPlaying = false;

    private void Awake()
    {
        _overlay = GetComponent<Image>();
        _banner = transform.GetChild(0).GetComponentInChildren<Image>();
        _label = GetComponentInChildren<TMP_Text>();
        _overlay.enabled = _banner.enabled = _label.enabled = false;
    }

    public void Play(Faction faction)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(GetPlaySequence(faction));
    }

    private IEnumerator GetPlaySequence(Faction faction)
    {
        _isPlaying = true;

        Color overlayColor = _overlay.color;
        overlayColor.a = 0;
        _overlay.color = overlayColor;
        _overlay.enabled = true;
        yield return _overlay.DOFade(_overlayAlpha, _transitionDuration).WaitForCompletion();

        Color bannerColor = faction.Color;
        bannerColor.a = _bannerAlpha;
        _banner.color = bannerColor;
        _banner.transform.localScale = new(0, 1, 1);
        _banner.enabled = true;
        yield return _banner.transform.DOScaleX(1, _transitionDuration).WaitForCompletion();

        _label.text = $"{faction.Name} Turn";
        _label.enabled = true;
        yield return new WaitForSeconds(_transitionDuration);

        yield return new WaitForSeconds(_holdDuration);

        _label.enabled = false;
        yield return new WaitForSeconds(_transitionDuration);
        yield return _banner.transform.DOScaleX(0, _transitionDuration).WaitForCompletion();
        yield return _overlay.DOFade(0, _transitionDuration).WaitForCompletion();

        _overlay.enabled = _banner.enabled = _label.enabled = false;
        _isPlaying = false;
    }
}
