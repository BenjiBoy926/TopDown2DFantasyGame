using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterTraversal : MonoBehaviour
{
    public IReadOnlyCollection<Vector3Int> TraversibleTiles => _traversibleTiles;

    [SerializeField] private int _range = 4;
    private Character _character;
    private readonly HashSet<Vector3Int> _traversibleTiles = new();

    public void RecalculateTraversibleTiles()
    {
        _traversibleTiles.Clear();

        Vector3Int homeCell = _character.GetCell();
        _traversibleTiles.Add(homeCell);

        for (int x = -_range; x <= _range; x++)
        {
            for (int y = -_range; y <= _range; y++)
            {
                Vector3Int offset = new(x, y, 0);
                Vector3Int newCell = homeCell + offset;
                _traversibleTiles.Add(newCell);
            }
        }
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}
