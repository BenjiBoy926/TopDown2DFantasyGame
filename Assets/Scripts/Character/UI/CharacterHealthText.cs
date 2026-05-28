using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class CharacterHealthText : MonoBehaviour
{
    [SerializeField] private float _halfThreshold = .5f;
    [SerializeField] private float _lowThreshold = .2f;
    [SerializeField] private Color _fullColor = Color.green;
    [SerializeField] private Color _halfColor = Color.yellow;
    [SerializeField] private Color _lowColor = Color.red;
    private CharacterStats _stats;
    private TMP_Text _label;

    public void Refresh()
    {
        int currentHealth = _stats.CurrentHealth;
        _label.text = currentHealth.ToString();

        int baseHealth = _stats.BaseHealth;
        int halfHealthThreshold = Mathf.CeilToInt(baseHealth * _halfThreshold);
        int lowHealthThreshold = Mathf.CeilToInt(baseHealth * _lowThreshold);
        if (currentHealth > halfHealthThreshold)
        {
            _label.color = _fullColor;
        }
        else if (currentHealth > lowHealthThreshold)
        {
            _label.color = _halfColor;
        }
        else
        {
            _label.color = _lowColor;
        }
    }

    private void Awake()
    {
        _stats = GetComponentInParent<CharacterStats>();
        _label = GetComponent<TMP_Text>();
    }
}