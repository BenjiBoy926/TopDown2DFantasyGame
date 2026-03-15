using System.Collections.Generic;
using UnityEngine;

public class CharacterRangeDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefab;
    private readonly List<GameObject> _tiles = new();
    private bool _isShown;

    public void Show(Character character)
    {
        if (_isShown) return;
        Debug.Log($"Show range for {name}", this);
        _isShown = true;
    }

    public void Hide()
    {
        if (!_isShown) return;
        Debug.Log($"Hide range for {name}", this);
        _isShown = false;
    }
}
