using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterHeartIcon : MonoBehaviour
{
    [SerializeField] private Sprite _healthFullSprite;
    [SerializeField] private Sprite _healthDamagedSprite;
    [SerializeField] private Sprite _healthDepletedSprite;
    private SpriteRenderer _renderer;

    public void ShowHealthPercent(float percent)
    {
        if (percent >= 1f)
        {
            _renderer.sprite = _healthFullSprite;
        }
        else if (percent > 0f)
        {
            _renderer.sprite = _healthDamagedSprite;
        }
        else
        {
            _renderer.sprite = _healthDepletedSprite;
        }
    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
}