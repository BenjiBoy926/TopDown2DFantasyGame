using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class CharacterHealthText : MonoBehaviour
{
    private TMP_Text _label;

    private void Awake()
    {
        _label = GetComponent<TMP_Text>();
    }

    public void ShowHealth(int health)
    {
        _label.text = health.ToString();
    }
}