using UnityEngine;

[RequireComponent(typeof(Battlefield))]
[RequireComponent(typeof(BattleTurn))]
public class Battle : MonoBehaviour
{
    public float CellWidth => _field.CellWidth;
    public float CellHeight => _field.CellHeight;
    public bool IsTurnChangeAnimationPlaying => _turn.IsAnimationPlaying;

    private Battlefield _field;
    private BattleTurn _turn;

    public void Register(Character obj)
    {
        _turn.Register(obj);
        _field.Register(obj);
    }

    public void Unregister(Character obj)
    {
        _turn.Unregister(obj);
        _field.Unregister(obj);
    }

    public void StartNextTurn()
    {
        _turn.StartNextTurn();
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        return _field.SnapToGrid(position);
    }

    public Vector3 CellToWorld(Vector3Int cell)
    {
        return _field.CellToWorld(cell);
    }

    public Vector3Int WorldToCell(Vector3 position)
    {
        return _field.WorldToCell(position);
    }

    public Character GetOccupant(Vector3Int cell)
    {
        return _field.GetOccupant(cell);
    }

    public Vector3Int GetCell(Character character)
    {
        return _field.GetCell(character);
    }

    public void RefreshOccupantCell(Character character)
    {
        _field.RefreshOccupantCell(character);
    }

    private void Awake()
    {
        _field = GetComponent<Battlefield>();
        _turn = GetComponent<BattleTurn>();
    }
}
