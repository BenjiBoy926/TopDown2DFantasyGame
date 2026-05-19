using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleState
{
    [SerializeField] private List<CharacterRecord> _records = new();

    public void AddRecord(Character character)
    {
        CharacterRecord record = CharacterRecord.Read(character);
        _records.Add(record);
    }
}