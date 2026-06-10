using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterMovePreview : MonoBehaviour
{
    private Character _character;
    private readonly List<Character> _activePreviews = new();

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void PreviewMove(Character other)
    {
        Clear();
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
        for (int i = 0; i < _activePreviews.Count; i++)
        {
            Character character = _activePreviews[i];
            character.ClearHealthPreview();
        }
        _activePreviews.Clear();
    }

    private void PreviewHeal(Character other)
    {
        other.PreviewHealth(other.BaseHealth);
        _activePreviews.Add(other);
    }

    private void PreviewAttack(Character other)
    {
        int thisHealth = _character.CalculateHealthAfterHitFrom(other);
        _character.PreviewHealth(thisHealth);

        int otherHealth = other.CalculateHealthAfterHitFrom(_character);
        other.PreviewHealth(otherHealth);

        _activePreviews.Add(_character);
        _activePreviews.Add(other);
    }
}