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

    private Image _overlay;
    private BattleTurnChangeBanner _banner;
    private TMP_Text _label;
    private bool _isPlaying = false;

    private void Awake()
    {
        _overlay = GetComponent<Image>();
        _banner = GetComponentInChildren<BattleTurnChangeBanner>();
        _label = GetComponentInChildren<TMP_Text>();
        _overlay.enabled = _label.enabled = false;
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

        yield return _banner.AnimateIn(faction);

        _label.text = $"{faction.Name} Turn";
        _label.enabled = true;
        yield return new WaitForSeconds(_transitionDuration);

        yield return new WaitForSeconds(_holdDuration);

        _label.enabled = false;
        yield return new WaitForSeconds(_transitionDuration);
        yield return _banner.AnimateOut();
        yield return _overlay.DOFade(0, _transitionDuration).WaitForCompletion();

        _overlay.enabled = _label.enabled = false;
        _isPlaying = false;
    }
}
