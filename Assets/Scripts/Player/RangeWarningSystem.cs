using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class RangeWarningSystem : MonoBehaviour
{
    [SerializeField] private RangeWarning _warningPrefab;
    private Player _player;
    private readonly List<RangeWarning> _activeWarnings = new();

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void Begin()
    {
        foreach (var character in _player.AllCharacters)
        {
            if (IsEnemyInPossibleRange(character))
            {
                AddNewWarning(character);
            }
        }
    }

    public void End()
    {
        foreach (var warning in _activeWarnings)
        {
            warning.End();
        }
        _activeWarnings.Clear();
    }

    private bool IsEnemyInPossibleRange(Character enemy)
    {
        return enemy && enemy.Faction != _player.Faction && IsWithinPossibleRange(_player.ActiveCharacter, enemy);
    }

    private static bool IsWithinPossibleRange(Character a, Character b)
    {
        int distance = CharacterRange.RectangularDistance(a.CurrentCell, b.CurrentCell);
        int range = a.TraversalRange + b.TraversalRange;
        return range >= (distance - 1);
    }

    private void AddNewWarning(Character attacker)
    {
        RangeWarning warning = Instantiate(_warningPrefab);
        warning.Begin(attacker, _player.ActiveCharacter);
        _activeWarnings.Add(warning);
    }
}
