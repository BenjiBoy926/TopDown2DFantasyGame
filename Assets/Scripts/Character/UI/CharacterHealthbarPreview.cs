using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterHealthbarPreview : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = .3f;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _renderer.color = Color.white;
        _renderer.DOFade(0, _fadeDuration).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        _renderer.DOKill();
    }
}