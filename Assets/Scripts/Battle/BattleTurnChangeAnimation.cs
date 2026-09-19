using System.Collections;
using UnityEngine;
using TMPro;

public class BattleTurnChangeAnimation : MonoBehaviour
{
    private readonly static WaitForSeconds _waitForSeconds3 = new(3);

    public bool IsPlaying => _isPlaying;

    private Overlay _overlay;
    private BattleTurnChangeBanner _banner;
    private BattleTurnChangeLabel _label;
    private bool _isPlaying = false;

    private void Awake()
    {
        _overlay = GetComponentInChildren<Overlay>();
        _banner = GetComponentInChildren<BattleTurnChangeBanner>();
        _label = GetComponentInChildren<BattleTurnChangeLabel>();
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
        yield return _label.Animate(faction);
        yield return _banner.AnimateOut();
        yield return _overlay.FadeOut();

        _isPlaying = false;
    }
}
