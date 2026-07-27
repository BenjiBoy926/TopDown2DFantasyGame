using UnityEngine;

[CreateAssetMenu(menuName = nameof(Faction))]
public class Faction : ScriptableObject
{
    public string Name => _name;
    public Color Color => _color;
    public bool CanBeRevived => _canBeRevived;
    public Transform CommanderTransform => _commander ? _commander.transform : null;
    public Vector3 CommanderPosition => CommanderTransform.position;

    [SerializeField] private string _name;
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private bool _canBeRevived = false;
    private Character _commander;

    public void RegisterCommander(Character commander)
    {
        _commander = commander;
    }

    public void UnregisterCommander(Character commander)
    {
        if (_commander == commander)
        {
            _commander = null;
        }
    }
}
