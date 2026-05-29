using UnityEngine;

public class CharacterHealthbar : MonoBehaviour
{
    private CharacterStats _stats;
    private SpriteRenderer _barSprite;

    public void Refresh()
    {
        int currentHealth = _stats.CurrentHealth;
        int baseHealth = _stats.BaseHealth;

        float healthPercent = (float)currentHealth / baseHealth;
        Vector3 scale = transform.localScale;
        scale.x = healthPercent;
        transform.localScale = scale;

        _barSprite.color = _stats.GetHealthColor();
    }

    private void Awake()
    {
        _stats = GetComponentInParent<CharacterStats>();
        _barSprite = GetComponentInChildren<SpriteRenderer>();
    }
}