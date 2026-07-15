using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterRange))]
[RequireComponent(typeof(CharacterMovePreview))]
[RequireComponent(typeof(CharacterAttackBehaviour))]
[RequireComponent(typeof(CharacterHurtBehaviour))]
[RequireComponent(typeof(CharacterDefendBehaviour))]
[RequireComponent(typeof(CharacterHealBehaviour))]
[RequireComponent(typeof(CharacterBeHealedBehaviour))]
[RequireComponent(typeof(CharacterCancelBehaviour))]
[RequireComponent(typeof(CharacterUndoRedoBehaviour))]
public class Character : MonoBehaviour
{
    // ── Events ───────────────────────────────────────────────────────────

    public static event Action<Character> MoveFinished = delegate { };

    // ── Properties ───────────────────────────────────────────────────────

    // Identity
    public Faction Faction => _faction;
    public bool IsRanged => _isRanged;
    public bool CanBeRevived => _faction.CanBeRevived;
    public bool IsInBattle => gameObject.activeSelf;

    // Spatial
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    public Vector2Int HomeCell => _battle.GetCell(this);
    public Vector2Int CurrentCell => _battle.WorldToCell(Position);
    public Vector2 CurrentCellCenter => _battle.SnapToGrid(Position);
    public float CellWidth => _battle.CellWidth;
    public float CellHeight => _battle.CellHeight;
    public Vector2 CellSize => new(_battle.CellWidth, _battle.CellHeight);

    // Stats
    public int BaseHealth => _stats.BaseHealth;
    public int CurrentHealth => _stats.CurrentHealth;
    public int Power => _stats.Power;
    public int TraversalRange => _stats.TraversalRange;

    // State
    public bool IsAbleToMove => !IsDead && !_hasMovedThisTurn;
    public bool IsDead => _stats.IsDead;
    public bool IsOneShotAnimationPlaying => _animator.IsOneShotAnimationPlaying;
    public static bool IsAnyCharacterActing => _actingCharacters.Count > 0;

    // Range
    public IReadOnlyCollection<Vector2Int> StayableCells => _range.StayableCells;
    public IReadOnlyCollection<Vector2Int> InteractableEdgeCells => _range.InteractableEdgeCells;

    // ── Fields ───────────────────────────────────────────────────────────

    [SerializeField] private Faction _faction;
    [SerializeField] private Color _usedMoveFadeColor = Color.gray;
    [SerializeField] private float _usedMoveFadeDuration = 0.35f;
    [SerializeField] private bool _isRanged;

    private CharacterAnimator _animator;
    private CharacterStats _stats;
    private CharacterRange _range;
    private CharacterMovePreview _preview;
    private CharacterAttackBehaviour _attackBehaviour;
    private CharacterHurtBehaviour _hurtBehaviour;
    private CharacterDefendBehaviour _defendBehaviour;
    private CharacterHealBehaviour _healBehaviour;
    private CharacterBeHealedBehaviour _beHealedBehaviour;
    private CharacterCancelBehaviour _cancelBehaviour;
    private CharacterUndoRedoBehaviour _undoRedoBehaviour;
    private Battle _battle;
    private bool _hasMovedThisTurn = false;
    // NOTE: static — must not carry state across scene reloads. Clear on scene unload if needed.
    private static readonly HashSet<Character> _actingCharacters = new();

    // ── Unity Lifecycle ──────────────────────────────────────────────────

    private void Awake()
    {
        _animator = GetComponentInChildren<CharacterAnimator>();
        _stats = GetComponent<CharacterStats>();
        _range = GetComponent<CharacterRange>();
        _preview = GetComponent<CharacterMovePreview>();
        _attackBehaviour = GetComponent<CharacterAttackBehaviour>();
        _hurtBehaviour = GetComponent<CharacterHurtBehaviour>();
        _defendBehaviour = GetComponent<CharacterDefendBehaviour>();
        _healBehaviour = GetComponent<CharacterHealBehaviour>();
        _beHealedBehaviour = GetComponent<CharacterBeHealedBehaviour>();
        _cancelBehaviour = GetComponent<CharacterCancelBehaviour>();
        _undoRedoBehaviour = GetComponent<CharacterUndoRedoBehaviour>();
    }

    private void OnEnable()
    {
        if (_battle)
        {
            _battle.Register(this);
        }
    }

    private void OnDisable()
    {
        if (_battle)
        {
            _battle.Unregister(this);
        }
        _actingCharacters.Remove(this);
    }

    // ── Animation ────────────────────────────────────────────────────────

    public void LookAt(Vector2 position)
    {
        Vector2 direction = position - Position;
        SetDirection(direction);
    }

    public void SetDirection(Vector2 direction)
    {
        _animator.SetDirection(direction);
    }

    public void SetIsRunning(bool isRunning)
    {
        _animator.SetIsRunning(isRunning);
    }

    public void PauseAnimation()
    {
        _animator.Pause();
    }

    public void ResumeAnimation()
    {
        _animator.Resume();
    }

    public void PlayIdleAnimation()
    {
        SetIsRunning(false);
        _animator.PlayLoopingAnimation();
    }

    public Coroutine PlayAttackAnimation()
    {
        return _animator.Attack();
    }

    public Coroutine PlayHurtAnimation()
    {
        return _animator.Hurt();
    }

    public Coroutine PlayDieAnimation()
    {
        return _animator.Die(); 
    }

    public YieldInstruction PlayAttackConnectShake()
    {
        return _hurtBehaviour.PlayAttackConnectShake();
    }

    public YieldInstruction FadeAlpha(float alpha, float duration, Ease ease)
    {
        return _animator.FadeAlpha(alpha, duration, ease);
    }

    public YieldInstruction PerformSpriteFade()
    {
        return PerformSpriteFade(_usedMoveFadeDuration);
    }

    public YieldInstruction PerformSpriteFade(float duration)
    {
        Color fadeColor = GetMoveFadeColor();
        return _animator.FadeColor(fadeColor, duration);
    }

    // ── Actions ──────────────────────────────────────────────────────────

    public Coroutine Attack(Character other)
    {
        StopAllCoroutines();
        IEnumerator sequence = _attackBehaviour.GetSequence(other);
        return StartCoroutine(sequence);
    }

    public Coroutine Defend()
    {
        StopAllCoroutines();
        IEnumerator sequence = _defendBehaviour.GetSequence();
        return StartCoroutine(sequence);
    }

    public Coroutine Heal(Character other)
    {
        StopAllCoroutines();
        IEnumerator sequence = _healBehaviour.GetSequence(other);
        return StartCoroutine(sequence);
    }

    public Coroutine BeHealed(Character other)
    {
        StopAllCoroutines();
        IEnumerator sequence = _beHealedBehaviour.GetSequence(other);
        return StartCoroutine(sequence);
    }

    public Coroutine Cancel()
    { 
        StopAllCoroutines();
        IEnumerator sequence = _cancelBehaviour.GetSequence();
        return StartCoroutine(sequence);
    }

    // ── Combat Sequences ─────────────────────────────────────────────────
    // These exist so sibling behaviours can drive each other without
    // holding direct references to one another.

    public IEnumerator GetMeleeSwipeSequence(Character other)
    {
        return _attackBehaviour.GetMeleeSwipeSequence(other);
    }

    public IEnumerator GetHurtSequence(Character attacker)
    {
        return _hurtBehaviour.GetHurtSequence(attacker);
    }

    // ── Movement ─────────────────────────────────────────────────────────

    public void BeginMove()
    {
        SetHasMovedThisTurn(true);
        RefreshCell();
        ClearMovePreview();
        SetIsActing(true);
    }

    public void EndMove()
    {
        SetIsActing(false);
        MoveFinished.Invoke(this);
    }

    public void RestoreMove()
    {
        SetHasMovedThisTurn(false);
        PerformSpriteFade(_usedMoveFadeDuration);
    }

    public Vector2 ClampToReachableCells(Vector2 input)
    {
        return _range.ClampToReachableCells(input);
    }

    public Vector2 ClampToStayableCells(Vector2 position)
    {
        return _range.ClampToStayableCells(position);
    }

    // ── Range Display ────────────────────────────────────────────────────

    public void RefreshRange()
    {
        _range.Refresh();
    }

    public void ShowTransparentRange()
    {
        _range.ShowTransparentRange();
    }

    public void ShowOpaqueRange()
    {
        _range.ShowOpaqueRange();
    }

    public void HideRange()
    {
        _range.Hide();
    }

    public bool IsReachable(Vector2Int cell)
    {
        return _range.IsReachable(cell);
    }

    public bool IsStayable(Vector2Int cell)
    {
        return _range.IsStayable(cell);
    }

    // ── Grid ─────────────────────────────────────────────────────────────

    public Vector2 CellToWorld(Vector2Int cell)
    {
        return _battle.CellToWorld(cell);
    }

    public Vector2Int WorldToCell(Vector2 position)
    {
        return _battle.WorldToCell(position);
    }

    public TileBase GetTile(Vector2Int cell)
    {
        return _battle.GetTile(cell);
    }

    public Character GetOccupant(Vector2Int cell)
    {
        return _battle.GetOccupant(cell);
    }

    public bool IsEnemyInCell(Vector2Int cell)
    {
        Character occupant = GetOccupant(cell);
        return occupant && occupant.Faction != _faction;
    }

    public bool CanStayInCell(Vector2Int cell)
    {
        Character occupant = GetOccupant(cell);
        return !occupant || occupant == this;
    }

    public void RefreshCell()
    {
        _battle.RefreshCell(this);
    }

    public bool IsPassable(Vector2Int cell)
    {
        return _range.IsPassable(cell);
    }

    public void FindPath(Vector2Int target, List<Vector2Int> path)
    {
        _range.FindPath(target, path);
    }

    // ── Health ───────────────────────────────────────────────────────────

    public void TakeDamageFrom(Character other)
    {
        _stats.TakeDamageFrom(other);
    }

    public void SetHealth(int health)
    {
        _stats.SetHealth(health);
    }

    public void RestoreHealth()
    {
        _stats.RestoreHealth();
    }

    public void PreviewHealth(int health)
    {
        _stats.PreviewHealth(health);
    }

    public void ClearHealthPreview()
    {
        _stats.ClearHealthPreview();
    }

    public int CalculateHealthAfterHitFrom(Character other)
    {
        return _stats.CalculateHealthAfterHitFrom(other);
    }

    // ── Move Preview ─────────────────────────────────────────────────────

    public void PreviewMove(Character other)
    {
        _preview.PreviewMove(other);
    }

    public void ClearMovePreview()
    {
        _preview.Clear();
    }

    // ── History ──────────────────────────────────────────────────────────

    public void RecordMoveWith(Character other)
    {
        _battle.Record(this, other);
    }

    public void RecordMove()
    {
        _battle.Record(this);
    }

    public CharacterState ReadState()
    {
        return new(_animator.GetDirection(), CurrentCell, _hasMovedThisTurn, _stats.CurrentHealth);
    }

    public IEnumerator GetApplyStateSequence(CharacterState state)
    {
        return _undoRedoBehaviour.GetApplyStateSequence(state);
    }

    public bool ShouldRemoveFromBattlefield()
    {
        return IsDead && !_faction.CanBeRevived;
    }

    // ── Setup ────────────────────────────────────────────────────────────

    public void SetBattle(Battle battle)
    {
        _battle = battle;
    }

    public void SetHasMovedThisTurn(bool hasMovedThisTurn)
    {
        _hasMovedThisTurn = hasMovedThisTurn;
    }

    public void SetIsActing(bool isActing)
    {
        if (isActing)
        {
            _actingCharacters.Add(this);
        }
        else
        {
            _actingCharacters.Remove(this);
        }
    }

    // ── Private ──────────────────────────────────────────────────────────

    private Color GetMoveFadeColor()
    {
        return _hasMovedThisTurn ? _usedMoveFadeColor : Color.white;
    }
}
