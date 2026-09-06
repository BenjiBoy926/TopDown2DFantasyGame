using UnityEngine;

public class CharacterHealthbar : MonoBehaviour
{
    private Character _character;
    private SpriteRenderer _barSprite;

    public void ShowCurrentHealth()
    {
        ShowHealth(_character.CurrentHealth);
    }

    public void ShowHealth(int health)
    {
        SetColor(health);
        SetFill(health);
    }

    public void SetColor(int currentHealth)
    {
        _barSprite.color = _character.GetHealthColor(currentHealth);
    }

    public void SetFill(int currentHealth)
    {
        int baseHealth = _character.BaseHealth;
        float healthPercent = (float)currentHealth / baseHealth;
        Vector3 scale = transform.localScale;
        scale.x = healthPercent;
        transform.localScale = scale;
    }

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _barSprite = GetComponentInChildren<SpriteRenderer>();
    }
}