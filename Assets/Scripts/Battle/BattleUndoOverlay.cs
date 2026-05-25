using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class BattleUndoOverlay : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = .2f;
    [SerializeField] private float _fadeAlpha = .1f;
    private Graphic _graphic;

    private void Awake()
    {
        _graphic = GetComponent<Graphic>();
        _graphic.color = new Color(_graphic.color.r, _graphic.color.g, _graphic.color.b, 0f);
    }

    public YieldInstruction FadeIn()
    {
        gameObject.SetActive(true);
        return FadeTo(_fadeAlpha);
    }

    public YieldInstruction FadeOut()
    {
        return FadeTo(0f);
    }

    private YieldInstruction FadeTo(float alpha)
    {
        return _graphic.DOFade(alpha, _fadeDuration).WaitForCompletion();
    }
}