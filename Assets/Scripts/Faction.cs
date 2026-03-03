using UnityEngine;

[CreateAssetMenu(menuName = nameof(Faction))]
public class Faction : ScriptableObject
{
    public string Name => _name;
    public Color Color => _color;

    [SerializeField] private string _name;
    [SerializeField] private Color _color = Color.white;
}
