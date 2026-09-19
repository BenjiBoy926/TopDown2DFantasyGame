using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class Overlay : MonoBehaviour
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

    public IEnumerator FadeOut()
    {
        yield return FadeTo(0f);
        gameObject.SetActive(false);
    }

    private YieldInstruction FadeTo(float alpha)
    {
        return _graphic.DOFade(alpha, _fadeDuration).WaitForCompletion();
    }
}