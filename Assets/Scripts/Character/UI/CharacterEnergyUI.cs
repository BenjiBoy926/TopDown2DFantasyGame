using UnityEngine;

public class CharacterEnergyUI : MonoBehaviour
{
    private Character _character;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
    }

    public void AnimateCurrentEnergy()
    {
        AnimateEnergy(_character.CurrentEnergy);
    }

    public void AnimateEnergy(int energy)
    {
        Debug.Log("Animating energy: " + energy);
    }

    public void ShowCurrentEnergy()
    {
        SetEnergy(_character.CurrentEnergy);
    }

    public void SetEnergy(int energy)
    {
        Debug.Log("Setting energy: " + energy);
    }
}