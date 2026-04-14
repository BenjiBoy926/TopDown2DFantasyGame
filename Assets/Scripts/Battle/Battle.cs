using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Battlefield))]
[RequireComponent(typeof(BattleTurn))]
public class Battle : MonoBehaviour
{
    public float CellWidth => _field.CellWidth;
    public float CellHeight => _field.CellHeight;
    public bool IsTurnChangeAnimationPlaying => _turn.IsAnimationPlaying;
    public Faction CurrentFactionTurn => _turn.CurrentFaction;

    [SerializeField] private float _startDelay = .5f;
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

    public Vector2 SnapToGrid(Vector2 position)
    {
        return _field.SnapToGrid(position);
    }

    public Vector2 CellToWorld(Vector2Int cell)
    {
        return _field.CellToWorld(cell);
    }

    public Vector2Int WorldToCell(Vector2 position)
    {
        return _field.WorldToCell(position);
    }

    public Character GetOccupant(Vector2Int cell)
    {
        return _field.GetOccupant(cell);
    }

    public Vector2Int GetCell(Character character)
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

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_startDelay);
        _turn.StartFirstTurn();
    }
}
