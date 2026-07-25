using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] private Vector2Int _extents = Vector2Int.one;
    [SerializeField, ReadOnly] private Battle _battle;
    [SerializeField, ReadOnly] private List<Character> _members = new();

    public void CollectMembers(Battle battle)
    {
        _battle = battle;
        for (int x = -_extents.x; x <= _extents.x; x++)
        {
            for (int y = -_extents.y; y <= _extents.y; y++)
            {
                Vector2Int offset = new(x, y);
                AddMemberAt(offset);
            }
        }
    }

    private void AddMemberAt(Vector2Int offset)
    {
        Vector2Int center = _battle.WorldToCell(transform.position);
        Vector2Int cell = center + offset;
        Character character = _battle.GetOccupant(cell);
        if (character != null)
        {
            _members.Add(character);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color color = Color.red;
        color.a = .2f;
        Color oldColor = Gizmos.color;
        Gizmos.color = color;

        Vector3 position = transform.position;
        Vector3 size = new Vector3(_extents.x * 2, _extents.y * 2, 0) + Vector3.one;
        Gizmos.DrawCube(position, size);       

        Gizmos.color = oldColor;
    }
}
