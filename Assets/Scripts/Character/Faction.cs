using UnityEngine;

[CreateAssetMenu(menuName = nameof(Faction))]
public class Faction : ScriptableObject
{
    public string Name => _name;
    public Color Color => _color;
    public bool CanBeRevived => _canBeRevived;

    [SerializeField] private string _name;
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private bool _canBeRevived = false;
}
