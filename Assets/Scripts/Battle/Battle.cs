using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BattleSetup))]
[RequireComponent(typeof(Battlefield))]
[RequireComponent(typeof(BattleTurn))]
[RequireComponent(typeof(BattleHistory))]
public class Battle : MonoBehaviour
{
    public float CellWidth => _field.CellWidth;
    public float CellHeight => _field.CellHeight;
    public bool IsTurnChangeAnimationPlaying => _turn.IsAnimationPlaying;
    public Faction CurrentFactionTurn => _turn.CurrentFaction;
    public Faction StartingFaction => _turn.StartingFaction;
    public IReadOnlyCollection<Character> AllCharacters => _allCharacters;

    private BattleSetup _setup;
    private Battlefield _field;
    private BattleTurn _turn;
    private BattleHistory _history;
    private readonly HashSet<Character> _allCharacters = new();

    public void Register(Character character)
    {
        _turn.AddFaction(character);
        _field.Register(character);
        _allCharacters.Add(character);
    }

    public void Unregister(Character character)
    {
        _field.Unregister(character);
        _allCharacters.Add(character);
    }

    public void StartFirstTurn()
    {
        _turn.StartFirstTurn();
    }

    public void StartNextTurn()
    {
        _turn.StartNextTurn();
    }

    public void StartTurn(Faction faction)
    {
        _turn.SetCurrentTurn(faction);
    }
    
    public void PlayTurnChangeAnimation()
    {
        _turn.PlayTurnChangeAnimation();
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

    public void RecordInitialState()
    {
        _history.RecordInitialState();
    }

    public void RecordTurnChange(List<Character> characters)
    {
        _history.RecordTurnChange(characters);
    }

    public void Record(Character a, Character b)
    {
        _history.Record(a, b);
    }

    public void Record(Character character)
    {
        _history.Record(character);
    }

    public void Undo()
    {
        _history.Undo();
    }

    public void Redo()
    {
        _history.Redo();
    }

    private void Awake()
    {
        _setup = GetComponent<BattleSetup>();
        _field = GetComponent<Battlefield>();
        _turn = GetComponent<BattleTurn>();
        _history = GetComponent<BattleHistory>();
    }

    private void Start()
    {
        _setup.Begin();
    }
}
