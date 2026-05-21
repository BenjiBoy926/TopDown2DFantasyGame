using System;
using UnityEngine;

[Serializable]
public struct CharacterState
{
    public readonly Vector2 Direction => _direction;
    public readonly Vector2Int Cell => _cell;
    public readonly bool HasMoved => _hasMoved;
    public readonly int Health => _health;

    [SerializeField] private Vector2 _direction;
    [SerializeField] private Vector2Int _cell;
    [SerializeField] private bool _hasMoved;
    [SerializeField] private int _health;

    public CharacterState(Vector2 direction, Vector2Int cell, bool hasMoved, int health)
    {
        _direction = direction;
        _cell = cell;
        _hasMoved = hasMoved;
        _health = health;
    }
}