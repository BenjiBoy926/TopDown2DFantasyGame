using DG.Tweening;
using UnityEngine;

public class RangeWarningScrollingMask : MonoBehaviour
{
    [SerializeField] private float _duration = 1;
    [SerializeField] private Ease _ease = Ease.InQuad;

    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        transform.DOLocalMoveY(1, _duration)
            .SetEase(_ease)
            .SetLoops(-1, LoopType.Restart);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}