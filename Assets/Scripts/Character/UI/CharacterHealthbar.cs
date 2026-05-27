using UnityEngine;

public class CharacterHealthbar : MonoBehaviour
{
    [SerializeField] private float _halfThreshold = .5f;
    [SerializeField] private float _lowThreshold = .2f;
    [SerializeField] private Color _fullColor = Color.green;
    [SerializeField] private Color _halfColor = Color.yellow;
    [SerializeField] private Color _lowColor = Color.red;
    private SpriteRenderer _barSprite;

    public void ShowHealthPercent(float percent)
    {
        Vector3 scale = transform.localScale;
        scale.x = percent;
        transform.localScale = scale;

        if (percent > _halfThreshold)
        {
            _barSprite.color = _fullColor;
        }
        else if (percent > _lowThreshold)
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
        _barSprite = GetComponentInChildren<SpriteRenderer>();
    }
}