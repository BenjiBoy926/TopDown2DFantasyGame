using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class CharacterHealthText : MonoBehaviour
{
    private CharacterStats _stats;
    private TMP_Text _label;

    public void Refresh()
    {
        int currentHealth = _stats.CurrentHealth;
        _label.text = currentHealth.ToString();
        _label.color = _stats.GetCurrentHealthColor();
    }

    private void Awake()
    {
        _stats = GetComponentInParent<CharacterStats>();
        _label = GetComponent<TMP_Text>();
    }
}