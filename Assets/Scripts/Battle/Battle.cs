using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    public Character GetCharacter(Vector2Int cell)
    {
        return _field.GetCharacter(cell);
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
