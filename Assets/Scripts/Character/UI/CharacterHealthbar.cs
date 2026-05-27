using UnityEngine;

public class CharacterHealthbar : MonoBehaviour
{
    [SerializeField] private float _halfThreshold = .5f;
    [SerializeField] private float _lowThreshold = .2f;
    [SerializeField] private Color _fullColor = Color.green;
    [SerializeField] private Color _halfColor = Color.yellow;
    [SerializeField] private Color _lowColor = Color.red;
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

        int halfHealthThreshold = Mathf.CeilToInt(baseHealth * _halfThreshold);
        int lowHealthThreshold = Mathf.CeilToInt(baseHealth * _lowThreshold);
        if (currentHealth > halfHealthThreshold)
        {
            _barSprite.color = _fullColor;
        }
        else if (currentHealth > lowHealthThreshold)
        {
            _barSprite.color = _halfColor;
        }
        else
        {
            _barSprite.color = _lowColor;
        }
    }

    private void Awake()
    {
        _stats = GetComponentInParent<CharacterStats>();
        _barSprite = GetComponentInChildren<SpriteRenderer>();
    }
}