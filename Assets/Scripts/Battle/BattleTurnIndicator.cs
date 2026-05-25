using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleTurnIndicator : MonoBehaviour
{
    [SerializeField] private float _growScale = 2;
    [SerializeField] private float _growDuration = 1;
    [SerializeField] private Ease _growEase = Ease.OutQuint;
    [SerializeField] private float _shrinkDuration = .3f;
    [SerializeField] private Ease _shrinkEase = Ease.Linear;
    private Image _image;
    private TMP_Text _label;

    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
        _label = GetComponentInChildren<TMP_Text>();
    }

    public void SetFaction(Faction faction)
    {
        Color color = faction.Color;
        _image.color = new(color.r, color.g, color.b, _image.color.a);
        _label.text = faction.Name;
        PlayGrowShrinkSequence();
    }

    private void PlayGrowShrinkSequence()
    {
        StopAllCoroutines();
        StartCoroutine(GetGrowShrinkSequence());
    }

    private IEnumerator GetGrowShrinkSequence()
    {
        yield return transform.DOScale(_growScale, _growDuration).SetEase(_growEase).WaitForCompletion();
        yield return transform.DOScale(1, _shrinkDuration).SetEase(_shrinkEase).WaitForCompletion();
    }
}