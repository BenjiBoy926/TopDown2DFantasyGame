using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CharacterHealthbar))]
public class CharacterHealthbarPreview : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = .3f;
    private CharacterHealthbar _healthbar;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _healthbar = GetComponent<CharacterHealthbar>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
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

    public void SetFill(int health)
    {
        _healthbar.SetFill(health);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}