using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(TMP_Text))]
public class BattleTurnChangeLabel : MonoBehaviour
{
    [SerializeField] private float _moveDuration = .35f;
    [SerializeField] private float _waitDuration = 1.5f;
    [SerializeField] private float _waitOffset = 30;

    private RectTransform _rectTransform;
    private TMP_Text _label;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _label = GetComponent<TMP_Text>();

        _rectTransform.anchoredPosition = new(-Screen.width, 0);
    }

    public IEnumerator Animate(Faction faction)
    {
        _label.text = $"{faction.Name} Turn";
        _rectTransform.anchoredPosition = new(-Screen.width, 0);
        yield return _rectTransform.DOAnchorPosX(-_waitOffset, _moveDuration).WaitForCompletion();
        yield return _rectTransform.DOAnchorPosX(_waitOffset, _waitDuration).WaitForCompletion();
        yield return _rectTransform.DOAnchorPosX(Screen.width, _moveDuration).WaitForCompletion();
    }
}