using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterTraversal : MonoBehaviour
{
    [SerializeField] private int _range = 4;
    private Character _character;
    private readonly HashSet<Vector3Int> _traversibleSpaces = new();

    public void RecalculateTraversibleSpaces()
    {
        _traversibleSpaces.Clear();
        _traversibleSpaces.Add(_character.GetCell());
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}
