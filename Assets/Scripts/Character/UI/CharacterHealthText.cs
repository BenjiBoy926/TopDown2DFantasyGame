using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class CharacterHealthText : MonoBehaviour
{
    private CharacterStats _stats;
    private TMP_Text _label;

    public void ShowHealth(int health)
    {
        _label.text = health.ToString();
        _label.color = _stats.GetHealthColor(health);
    }

    private void Awake()
    {
        _stats = GetComponentInParent<CharacterStats>();
        _label = GetComponent<TMP_Text>();
    }
}