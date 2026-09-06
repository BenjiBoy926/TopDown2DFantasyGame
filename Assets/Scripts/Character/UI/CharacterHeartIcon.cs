using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterHeartIcon : MonoBehaviour
{
    [SerializeField] private Sprite _healthFullSprite;
    [SerializeField] private Sprite _healthDamagedSprite;
    [SerializeField] private Sprite _healthDepletedSprite;
    private SpriteRenderer _renderer;

    public void ShowHealth(CharacterUIStyle style, int currentHealth, int baseHealth)
    {
        Sprite heartSprite = style.GetHeartSprite(currentHealth, baseHealth);
        _renderer.sprite = heartSprite;
    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
}