using System;
using UnityEngine;

[Serializable]
public struct CharacterState
{
    public readonly Vector2 Direction => _direction;
    public readonly Vector2Int Cell => _cell;
    public readonly int Health => _health;
    public readonly int Energy => _energy;

    [SerializeField] private Vector2 _direction;
    [SerializeField] private Vector2Int _cell;
    [SerializeField] private int _health;
    [SerializeField] private int _energy;

    public CharacterState(Vector2 direction, Vector2Int cell, int health, int energy)
    {
        _direction = direction;
        _cell = cell;
        _health = health;
        _energy = energy;
    }
}