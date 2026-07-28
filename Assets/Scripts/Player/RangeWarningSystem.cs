using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class RangeWarningSystem : MonoBehaviour
{
    private Player _player;
    private readonly HashSet<Character> _enemiesWithinPossibleRange = new();

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void Begin()
    {
        _enemiesWithinPossibleRange.Clear();
        foreach (var character in _player.AllCharacters)
        {
            if (IsEnemyInPossibleRange(character))
            {
                _enemiesWithinPossibleRange.Add(character);
            }
        }
    }

    public void Refresh()
    {
        foreach (var enemy in _enemiesWithinPossibleRange)
        {
            enemy.RefreshRange();
            enemy.HideRange();
            enemy.ShowTransparentRange();
        }
    }

    public void End()
    {
        foreach (var enemy in _enemiesWithinPossibleRange)
        {
            enemy.HideRange();
        }
        _enemiesWithinPossibleRange.Clear();
    }

    private bool IsEnemyInPossibleRange(Character enemy)
    {
        return enemy && enemy.Faction != _player.Faction && IsWithinPossibleRange(_player.ActiveCharacter, enemy);
    }

    private static bool IsWithinPossibleRange(Character a, Character b)
    {
        int distance = CharacterRange.RectangularDistance(a.CurrentCell, b.CurrentCell);
        int range = a.TraversalRange + b.TraversalRange;
        return distance <= range;
    }
}
