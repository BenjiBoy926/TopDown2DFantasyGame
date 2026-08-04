using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class RangeWarningSystem : MonoBehaviour
{
    [SerializeField] private RangeWarning _warningPrefab;
    private Player _player;
    private Character _target;
    private readonly List<RangeWarning> _activeWarnings = new();

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void SetTarget(Character target)
    {
        if (target == _target)
            return;

        RemoveAllWarnings();
        _target = target;
        if (_target)
        {
            AddAllWarnings();
        }
    }

    private void AddAllWarnings()
    {
        foreach (var character in _player.AllCharacters)
        {
            if (IsEnemyInPossibleRange(character))
            {
                AddNewWarning(character);
            }
        }
    }

    private void RemoveAllWarnings()
    {
        foreach (var warning in _activeWarnings)
        {
            warning.End();
        }
        _activeWarnings.Clear();
    }

    private bool IsEnemyInPossibleRange(Character enemy)
    {
        return enemy && enemy.Faction != _target.Faction && IsWithinPossibleRange(_target, enemy);
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
        warning.Begin(attacker, _target);
        _activeWarnings.Add(warning);
    }
}
