using UnityEngine;

public class CharacterHealthColor : MonoBehaviour
{
    [SerializeField] private float _halfThreshold = .5f;
    [SerializeField] private float _lowThreshold = .2f;
    [SerializeField] private Color _fullColor = Color.green;
    [SerializeField] private Color _halfColor = Color.yellow;
    [SerializeField] private Color _lowColor = Color.red;

    public Color GetColor(int currentHealth, int baseHealth)
    {
        int halfHealthThreshold = Mathf.CeilToInt(baseHealth * _halfThreshold);
        int lowHealthThreshold = Mathf.CeilToInt(baseHealth * _lowThreshold);
        if (currentHealth > halfHealthThreshold)
        {
            return _fullColor;
        }
        else if (currentHealth > lowHealthThreshold)
        {
            return _halfColor;
        }
        else
        {
            return _lowColor;
        }
    }
}