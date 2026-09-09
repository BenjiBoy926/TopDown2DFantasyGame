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

        InteractionResult result = PredictInteractionResult(other);
        _character.Preview(result.Interactor);
        other.Preview(result.Target);

        _activePreviews.Add(_character);
        _activePreviews.Add(other);
    }

    public void Clear()
    {
        for (int i = 0; i < _activePreviews.Count; i++)
        {
            Character character = _activePreviews[i];
            character.ClearPreview();
        }
        _activePreviews.Clear();
    }

    public InteractionResult PredictInteractionResult(Character other)
    {
        if (other.Faction == _character.Faction)
        {
            return PredictHealResult(other);
        }
        else
        {
            return PredictAttackResult(other);
        }
    }
    
    private InteractionResult PredictHealResult(Character other)
    {
        CharacterInfo selfInfo = new(_character.CurrentHealth, _character.CurrentEnergy - 1);
        
        int otherHealth = other.BaseHealth;
        int otherEnergy = other.IsDead ? other.CurrentEnergy - 1 : other.CurrentEnergy;
        CharacterInfo otherInfo = new(otherHealth, otherEnergy);
        
        return new(selfInfo, otherInfo);
    }

    private InteractionResult PredictAttackResult(Character other)
    {
        int otherHealth = CalculateHealthAfterHit(other.CurrentHealth, _character.CurrentPower);
        CharacterInfo otherInfo = new(otherHealth, other.CurrentEnergy);
        
        int selfHealth;
        if (_character.IsRanged || other.IsRanged)
        {
            selfHealth = _character.CurrentHealth;
        }
        else
        {
            selfHealth = CalculateHealthAfterHit(_character.CurrentHealth, other.CurrentPower);
        }
        CharacterInfo selfInfo = new(selfHealth, _character.CurrentEnergy - 1);
        
        return new(selfInfo, otherInfo);
    }

    private int CalculateHealthAfterHit(int health, int power)
    {
        return Mathf.Max(0, health - power);
    }
}