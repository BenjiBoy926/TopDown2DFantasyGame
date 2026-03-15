using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterTraversal))]
public class CharacterRangeDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefab;
    private CharacterTraversal _traversal;
    private readonly List<GameObject> _tiles = new();

    public void Show()
    {

    }

    public void Hide()
    {

    }

    private void Awake()
    {
        _traversal = GetComponent<CharacterTraversal>();
    }
}
