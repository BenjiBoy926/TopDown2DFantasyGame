using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    public List<Character> Members => _members;
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

        if (ShouldWakeUp())
        {
            WakeUp();
        }
    }

    public bool ShouldWakeUp()
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

    public bool Contains(Character character)
    {
        return _members.Contains(character);
    }

    private bool IsEnemyInRange(Character member)
    {
        member.RefreshRange();
        foreach (var cell in member.AllCellsInRange)
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
