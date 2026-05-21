using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleState
{
    public int RecordCount => _records.Count;
    public Faction CurrentTurn => _currentTurn;
    public virtual bool IsTurnChange => false;

    [SerializeField] private Faction _currentTurn;
    [SerializeField] private List<CharacterRecord> _records = new();

    public void SetCurrentTurn(Faction currentTurn)
    {
        _currentTurn = currentTurn;
    }

    public void AddRecord(Character character)
    {
        CharacterRecord record = CharacterRecord.Read(character);
        _records.Add(record);
    }

    public CharacterRecord GetRecord(int i)
    {
        return _records[i];
    }

    public IEnumerator GetAllApplySequences()
    {
        foreach (var record in _records)
        {
            yield return record.GetApplySequence();
        }
    }

    public bool TryGetRecord(Character character, out CharacterRecord outRecord)
    {
        for (int i = 0; i < _records.Count; i++)
        {
            CharacterRecord record = _records[i];
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