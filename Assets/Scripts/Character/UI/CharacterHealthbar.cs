using UnityEngine;

public class CharacterHealthbar : MonoBehaviour
{
    private CharacterStats _stats;
    private SpriteRenderer _barSprite;

    public void ShowCurrentHealth()
    {
        ShowHealth(_stats.CurrentHealth);
    }

    public void ShowHealth(int health)
    {
        SetColor(health);
        SetFill(health);
    }

    public void SetColor(int currentHealth)
    {
        _barSprite.color = _stats.GetHealthColor(currentHealth);
    }

    public void SetFill(int currentHealth)
    {
        int baseHealth = _stats.BaseHealth;
        float healthPercent = (float)currentHealth / baseHealth;
        Vector3 scale = transform.localScale;
        scale.x = healthPercent;
        transform.localScale = scale;
    }

    private void Awake()
    {
        _stats = GetComponentInParent<CharacterStats>();
        _barSprite = GetComponentInChildren<SpriteRenderer>();
    }
}