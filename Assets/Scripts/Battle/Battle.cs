using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BattleSetup))]
[RequireComponent(typeof(Battlefield))]
[RequireComponent(typeof(BattleTurn))]
[RequireComponent(typeof(BattleRecord))]
public class Battle : MonoBehaviour
{
    public float CellWidth => _field.CellWidth;
    public float CellHeight => _field.CellHeight;
    public bool IsTurnChangeAnimationPlaying => _turn.IsAnimationPlaying;
    public Faction CurrentFactionTurn => _turn.CurrentFaction;

    private BattleSetup _setup;
    private Battlefield _field;
    private BattleTurn _turn;
    private BattleRecord _record;

    public void Register(Character character)
    {
        _field.Register(character);
        _turn.Register(character);
    }

    public void Unregister(Character character)
    {
        _field.Unregister(character);
        _turn.Unregister(character);
    }

    public void StartFirstTurn()
    {
        _turn.StartFirstTurn();
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

    public TileBase GetTile(Vector2Int cell)
    {
        return _field.GetTile(cell);
    }

    public void RefreshCell(Character character)
    {
        _field.RefreshCell(character);
    }

    public void Record(Character a, Character b)
    {
        _record.Record(a, b);
    }

    public void Record(Character character)
    {
        _record.Record(character);
    }

    public void Undo()
    {
        _record.Undo();
    }

    public void Redo()
    {
        _record.Redo();
    }

    private void Awake()
    {
        _setup = GetComponent<BattleSetup>();
        _field = GetComponent<Battlefield>();
        _turn = GetComponent<BattleTurn>();
        _record = GetComponent<BattleRecord>();
    }

    private void Start()
    {
        _setup.Begin();
    }
}
