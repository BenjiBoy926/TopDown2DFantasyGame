using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleState
{
    public IReadOnlyList<CharacterRecord> Records => _records;

    [SerializeField] private List<CharacterRecord> _records = new();

    public void AddRecord(Character character)
    {
        CharacterRecord record = CharacterRecord.Read(character);
        _records.Add(record);
    }

    public void ApplyAll()
    {
        foreach (var record in _records)
        {
            record.Apply();
        }
    }

    public bool TryGetRecord(Character character, out CharacterRecord outRecord)
    {
        foreach (var record in _records)
        {
            if (record.Character == character)
            {
                outRecord = record;
                return true;
            }
        }
        outRecord = default;
        return false;
    }
}