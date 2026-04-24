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

    public void Register(Character character)
    {
        _turn.Register(character);
    }

    public void Unregister(Character character)
    {
        _turn.Unregister(character);
    }

    public void Register(Obstacle obstacle)
    {
        _field.Register(obstacle);
    }

    public void Unregister(Obstacle obstacle)
    {
        _field.Unregister(obstacle);
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

    public Obstacle GetObstacle(Vector2Int cell)
    {
        return _field.GetObstacle(cell);
    }

    public Vector2Int GetCell(Obstacle obstacle)
    {
        return _field.GetCell(obstacle);
    }

    public void RefreshCell(Obstacle obstacle)
    {
        _field.RefreshCell(obstacle);
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
