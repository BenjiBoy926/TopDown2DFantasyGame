using System.Collections;
using UnityEngine;
using TMPro;

public class BattleTurnChangeAnimation : MonoBehaviour
{
    private readonly static WaitForSeconds _waitForSeconds3 = new(3);

    public bool IsPlaying => _isPlaying;

    [SerializeField] private float _transitionDuration = .35f;
    [SerializeField] private float _holdDuration = 1.5f;

    private Overlay _overlay;
    private BattleTurnChangeBanner _banner;
    private TMP_Text _label;
    private bool _isPlaying = false;

    private void Awake()
    {
        _overlay = GetComponentInChildren<Overlay>();
        _banner = GetComponentInChildren<BattleTurnChangeBanner>();
        _label = GetComponentInChildren<TMP_Text>();
        _label.enabled = false;
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

        yield return _overlay.FadeIn();
        yield return _banner.AnimateIn(faction);

        _label.text = $"{faction.Name} Turn";
        _label.enabled = true;
        yield return new WaitForSeconds(_transitionDuration);

        yield return new WaitForSeconds(_holdDuration);

        _label.enabled = false;
        yield return new WaitForSeconds(_transitionDuration);
        yield return _banner.AnimateOut();
        yield return _overlay.FadeOut();

        _label.enabled = false;
        _isPlaying = false;
    }
}
