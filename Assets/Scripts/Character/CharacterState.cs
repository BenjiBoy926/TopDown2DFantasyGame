using System;
using UnityEngine;

[Serializable]
public struct CharacterState
{
    public readonly Vector2 Direction => _direction;
    public readonly Vector2Int Cell => _cell;
    public readonly int Health => _info.Health;
    public readonly int Energy => _info.Energy;

    [SerializeField] private Vector2 _direction;
    [SerializeField] private Vector2Int _cell;
    [SerializeField] private CharacterInfo _info;

    public CharacterState(Vector2 direction, Vector2Int cell, CharacterInfo info)
    {
        _direction = direction;
        _cell = cell;
        _info = info;
    }
}