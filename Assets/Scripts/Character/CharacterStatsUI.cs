using TMPro;
using UnityEngine;

public class CharacterStatsUI : MonoBehaviour
{
    [SerializeField] private Sprite _healthFullSprite;
    [SerializeField] private Sprite _healthDamagedSprite;
    [SerializeField] private Sprite _healthDepletedSprite;
    private TMP_Text _healthText;
    private SpriteRenderer _renderer;

    public void ShowHealth(int currentHealth, int baseHealth)
    {
        _healthText.text = currentHealth.ToString();
        if (currentHealth == baseHealth)
        {
            _renderer.sprite = _healthFullSprite;
        }
        else if (currentHealth > 0)
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
        _healthText = GetComponentInChildren<TMP_Text>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }
}