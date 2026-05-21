using System;
using UnityEngine;

[Serializable]
public struct CharacterState
{
    public readonly CharacterAnimator.HorizontalDirectionType HorizontalDirection => _horizontalDirection;
    public readonly CharacterAnimator.VerticalDirectionType VerticalDirection => _verticalDirection;
    public readonly Vector2Int Cell => _cell;
    public readonly bool HasMoved => _hasMoved;
    public readonly int Health => _health;

    [SerializeField] private CharacterAnimator.HorizontalDirectionType _horizontalDirection;
    [SerializeField] private CharacterAnimator.VerticalDirectionType _verticalDirection;
    [SerializeField] private Vector2Int _cell;
    [SerializeField] private bool _hasMoved;
    [SerializeField] private int _health;

    public CharacterState(CharacterAnimator.HorizontalDirectionType horizontalDirection, CharacterAnimator.VerticalDirectionType verticalDirection, Vector2Int cell, bool hasMoved, int health)
    {
        _horizontalDirection = horizontalDirection;
        _verticalDirection = verticalDirection;
        _cell = cell;
        _hasMoved = hasMoved;
        _health = health;
    }
}