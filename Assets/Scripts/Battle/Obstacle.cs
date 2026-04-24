using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    public Character Character => _character;
    public Faction Faction => _character ? _character.Faction : null;

    private Character _character;

    private void Awake()
    {
        // Not all obstacles will have a character
#pragma warning disable UNT0039 // Use RequireComponent attribute when self-invoking GetComponent
        _character = GetComponent<Character>();
#pragma warning restore UNT0039 // Use RequireComponent attribute when self-invoking GetComponent
    }
}