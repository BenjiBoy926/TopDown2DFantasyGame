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
    public Faction PlayerFaction => _player.Faction;
    public IReadOnlyCollection<Character> AllCharacters => _allCharacters;
    public bool IsCameraGrabbed => _camera.IsGrabbed;

    private BattleSetup _setup;
    private Battlefield _field;
    private BattleTurn _turn;
    private BattleHistory _history;
    private BattleCamera _camera;
    private Player _player;
    private readonly HashSet<Character> _allCharacters = new();

    private void Awake()
    {
        _setup = GetComponent<BattleSetup>();
        _field = GetComponent<Battlefield>();
        _turn = GetComponent<BattleTurn>();
        _history = GetComponent<BattleHistory>();
        _camera = GetComponentInChildren<BattleCamera>();
        _player = GetComponentInChildren<Player>();
    }

    private void Start()
    {
        _setup.Begin();
    }


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

    public void StartPlayerTurn()
    {
        _turn.StartTurn(PlayerFaction);
    }

    public void StartNextTurn()
    {
        _turn.StartNextTurn();
    }

    public void StartTurn(Faction faction)
    {
        _turn.SetCurrentTurn(faction);
    }

    public void GetCharactersInFaction(Faction faction, List<Character> result)
    {
        _turn.GetCharactersInFaction(faction, result);
    }

    public int CountMoveableCharacters(Faction faction)
    {
        return _turn.CountMoveableCharacters(faction);
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

    public void RecordTurnChange(IReadOnlyCollection<Character> characters, Faction faction)
    {
        _history.RecordTurnChange(characters, faction);
    }

    public void Record(Character a, Character b)
    {
        _history.Record(a, b);
    }

    public void Record(Character character)
    {
        _history.Record(character);
    }

    public Coroutine Undo()
    {
        return _history.Undo();
    }

    public Coroutine Redo()
    {
        return _history.Redo();
    }

    // Camera ===

    public void GrabCamera(Vector2 worldPosition)
    {
        _camera.Grab(worldPosition);
    }

    public void UpdateCameraGrab(Vector2 screenPosition)
    {
        _camera.UpdateGrab(screenPosition);
    }

    public void ReleaseCamera()
    {
        _camera.Release();
    }

    public void CameraFollow(Transform target)
    {
        _camera.Follow(target);
    }

    public void CameraUnfollow()
    {
        _camera.Unfollow();
    }

    public Vector2 ScreenToWorld(Vector2 screenPosition)
    {
        return _camera.ScreenToWorld(screenPosition);
    }

    public void IncludeInView(Vector2 position)
    {
        _camera.IncludeInView(position);
    }

    public void ChangeZoom(float zoom)
    {
        _camera.ChangeZoom(zoom);
    }

    public void SetZoom(float zoom)
    {
        _camera.SetZoom(zoom);
    }

    public void ZoomIn()
    {
        _camera.ZoomIn();
    }

    public void ZoomOut()
    {
        _camera.ZoomOut();
    }
}
