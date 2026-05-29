using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterMovePreview : MonoBehaviour
{
    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void PreviewMove(Character other)
    {
        if (_character.Faction == other.Faction)
        {
            PreviewHeal(other);
        }
        else
        {
            PreviewAttack(other);
        }
    }

    public void Clear()
    {
        Debug.Log("Clearing move preview");
    }

    private void PreviewHeal(Character other)
    {
        Debug.Log("Previewing heal on " + other.name);
    }

    private void PreviewAttack(Character other)
    {
        Debug.Log("Previewing attack on " + other.name);
    }
}