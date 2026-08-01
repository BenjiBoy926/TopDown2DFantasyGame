using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    public IReadOnlyList<Character> Members => _members;
    public bool IsAwake => _isAwake;

    [SerializeField, ReadOnly] private List<Character> _members = new();
    [SerializeField, ReadOnly] private bool _isAwake;

    private void Awake()
    {
        _members = new(GetComponentsInChildren<Character>());
    }

    public void Refresh()
    {
        if (_isAwake) 
            return;

        if (ShouldBeAwake())
        {
            WakeUp();
        }
    }

    private bool ShouldBeAwake()
    {
        for (int i = 0; i < _members.Count; i++)
        {
            Character member = _members[i];
            if (IsEnemyInRange(member))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsEnemyInRange(Character member)
    {
        member.RefreshRange();
        foreach (var cell in member.ReachableCells)
        {
            if (member.IsEnemyInCell(cell, out _))
            {
                return true;
            }
        }
        return false;
    }

    private void WakeUp()
    {
        _isAwake = true;
    }
}
