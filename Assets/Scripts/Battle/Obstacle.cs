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

    private Battle _battle;
    private Character _character;

    private void Awake()
    {
        // Not all obstacles will have a character
#pragma warning disable UNT0039 // Use RequireComponent attribute when self-invoking GetComponent
        _character = GetComponent<Character>();
#pragma warning restore UNT0039 // Use RequireComponent attribute when self-invoking GetComponent
    }

    private void OnEnable()
    {
        if (_battle)
        {
            _battle.Register(this);
        }
    }

    private void OnDisable()
    {
        if (_battle)
        {
            _battle.Unregister(this);
        }
    }

    private void Start()
    {
        _battle = GetComponentInParent<Battle>();
        if (_battle)
        {
            _battle.Register(this);
        }
    }
}