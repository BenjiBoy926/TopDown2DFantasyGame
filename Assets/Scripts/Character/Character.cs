using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterRange))]
[RequireComponent(typeof(GridSearch))]
[RequireComponent(typeof(CharacterWalker))]
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

    // Self
    public bool IsInBattle => gameObject.activeSelf;
    public bool IsRanged => _isRanged;
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    public static bool IsAnyCharacterActing => _actingCharacters.Count > 0;
    public bool IsAbleToMove => !IsDead && _stats.CurrentEnergy > 0;

    // Faction
    public Faction Faction => _faction;
    public bool CanBeRevived => _faction.CanBeRevived;

    // Animator
    public bool IsOneShotAnimationPlaying => _animator.IsOneShotAnimationPlaying;

    // Stats
    public int BaseHealth => _stats.BaseHealth;
    public int BaseEnergy => _stats.BaseEnergy;
    public int CurrentHealth => _stats.CurrentHealth;
    public int CurrentPower => _stats.CurrentPower;
    public int CurrentEnergy => _stats.CurrentEnergy;
    public int TraversalRange => _stats.TraversalRange;
    public bool IsDead => _stats.IsDead;

    // Range
    public HashSet<Vector2Int> AllCellsInRange => _range.AllCells;

    // Battle
    public float CellWidth => _battle.CellWidth;
    public float CellHeight => _battle.CellHeight;
    public Vector2 CellSize => new(_battle.CellWidth, _battle.CellHeight);
    public Vector2 CurrentCellCenter => _battle.SnapToGrid(Position);
    public Vector2Int CurrentCell => _battle.WorldToCell(Position);

    // ── Fields ───────────────────────────────────────────────────────────

    [SerializeField] private Faction _faction;
    [SerializeField] private Color _usedMoveFadeColor = Color.gray;
    [SerializeField] private float _usedMoveFadeDuration = 0.35f;
    [SerializeField] private bool _isRanged;

    private CharacterAnimator _animator;
    private CharacterUI _ui;
    private CharacterStats _stats;
    private CharacterRange _range;
    private GridSearch _gridSearch;
    private CharacterWalker _walker;
    private CharacterMovePreview _preview;
    private CharacterAttackBehaviour _attackBehaviour;
    private CharacterHurtBehaviour _hurtBehaviour;
    private CharacterDefendBehaviour _defendBehaviour;
    private CharacterHealBehaviour _healBehaviour;
    private CharacterBeHealedBehaviour _beHealedBehaviour;
    private CharacterCancelBehaviour _cancelBehaviour;
    private CharacterUndoRedoBehaviour _undoRedoBehaviour;
    private Battle _battle;
    // NOTE: static — must not carry state across scene reloads. Clear on scene unload if needed.
    private static readonly HashSet<Character> _actingCharacters = new();

    // ── Unity Lifecycle ──────────────────────────────────────────────────

    private void Awake()
    {
        _animator = GetComponentInChildren<CharacterAnimator>();
        _ui = GetComponentInChildren<CharacterUI>();
        _stats = GetComponent<CharacterStats>();
        _range = GetComponent<CharacterRange>();
        _gridSearch = GetComponent<GridSearch>();
        _walker = GetComponent<CharacterWalker>();
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

    // ── Self ─────────────────────────────────────────────────────────────

    public void SetBattle(Battle battle)
    {
        _battle = battle;
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

    public void BeginMove()
    {
        RefreshCell();
        ClearInteractionPreview();
        SetIsActing(true);
    }

    public void EndMove()
    {
        ChangeEnergy(-1);
        SetIsActing(false);
        MoveFinished.Invoke(this);
    }

    public void RestoreMove()
    {
        RefillEnergy();
        PerformSpriteFade(_usedMoveFadeDuration);
    }

    public void LookAt(Vector2 position)
    {
        Vector2 direction = position - Position;
        SetDirection(direction);
    }

    public void PlayIdleAnimation()
    {
        SetIsRunning(false);
        _animator.PlayLoopingAnimation();
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

    public bool IsEnemyInCell(Vector2Int cell, out Character enemy)
    {
        enemy = GetOccupant(cell);
        return enemy && enemy.Faction != _faction;
    }

    public bool IsAllyInCell(Vector2Int cell)
    {
        Character occupant = GetOccupant(cell);
        return occupant && occupant != this && occupant.Faction == _faction;
    }

    public bool CouldStayInCell(Vector2Int cell)
    {
        Character occupant = GetOccupant(cell);
        return !occupant || occupant == this;
    }

    public bool ShouldRemoveFromBattlefield()
    {
        return IsDead && !_faction.CanBeRevived;
    }

    public CharacterState ReadState()
    {
        return new(_animator.GetDirection(), CurrentCell, new(_stats.CurrentHealth, _stats.CurrentEnergy));
    }

    private Color GetMoveFadeColor()
    {
        return IsAbleToMove ? Color.white : _usedMoveFadeColor;
    }

    // ── Faction ──────────────────────────────────────────────────────────

    public void RegisterAsCommander()
    {
        _faction.RegisterCommander(this);
    }

    public void UnregisterAsCommander()
    {
        _faction.UnregisterCommander(this);
    }

    // ── Animator ─────────────────────────────────────────────────────────

    public void PauseAnimation()
    {
        _animator.Pause();
    }

    public void ResumeAnimation()
    {
        _animator.Resume();
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

    public void SetDirection(Vector2 direction)
    {
        _animator.SetDirection(direction);
    }

    public void SetIsRunning(bool isRunning)
    {
        _animator.SetIsRunning(isRunning);
    }

    public YieldInstruction FadeAlpha(float alpha, float duration, Ease ease)
    {
        return _animator.FadeAlpha(alpha, duration, ease);
    }

    // ── UI ───────────────────────────────────────────────────────────────

    public void Preview(CharacterInfo info)
    {
        _ui.Preview(info);
    }

    public void ClearPreview()
    {
        _ui.ClearPreview();
    }

    public void ShowCurrentHealth()
    {
        _ui.ShowCurrentHealth();
    }

    public void AnimateCurrentEnergy()
    {
        _ui.AnimateCurrentEnergy();
    }

    public void ShowCurrentEnergy()
    {
        _ui.ShowCurrentEnergy();
    }

    public void ShowPower()
    {
        _ui.ShowPower();
    }

    public void ShakeHealthUI()
    {
        _ui.ShakeHealthUI();
    }

    // ── Stats ────────────────────────────────────────────────────────────

    public void TakeDamageFrom(Character other)
    {
        _stats.TakeDamageFrom(other);
    }

    public void RestoreHealth()
    {
        _stats.RestoreHealth();
    }

    public int CalculateHealthAfterHitFrom(Character other)
    {
        return _stats.CalculateHealthAfterHitFrom(other);
    }

    public void SetHealth(int health)
    {
        _stats.SetHealth(health);
    }

    public void ChangeEnergy(int delta)
    {
        _stats.ChangeEnergy(delta);
    }

    public void ZeroEnergy()
    {
        _stats.ZeroEnergy();
    }

    public void RefillEnergy()
    {
        _stats.RefillEnergy();
    }

    public void SetEnergy(int energy)
    {
        _stats.SetEnergy(energy);
    }

    // ── Range ────────────────────────────────────────────────────────────

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

    public Vector2 ClampToStayableCells(Vector2 position)
    {
        return _range.ClampToStayableCells(position);
    }

    public Vector2 ClampToReachableCells(Vector2 input)
    {
        return _range.ClampToReachableCells(input);
    }

    public bool CouldWalkThroughCell(Vector2Int cell)
    {
        return _range.CouldWalkThroughCell(cell);
    }

    public bool IsInRange(Vector2Int cell)
    {
        return _range.Contains(cell);
    }

    public bool IsReachable(Vector2Int cell)
    {
        return _range.IsReachable(cell);
    }

    public bool IsStayable(Vector2Int cell)
    {
        return _range.IsStayable(cell);
    }

    // ── Grid Search ──────────────────────────────────────────────────────

    public GridSearchResult SearchGrid(GridSearchStrategy strategy)
    {
        return _gridSearch.Search(strategy);
    }

    // ── Walker ───────────────────────────────────────────────────────────

    public Coroutine WalkToNodeClamped(Node node)
    {
        return _walker.WalkToNodeClamped(node);
    }

    // ── Move Preview ─────────────────────────────────────────────────────

    public void PreviewInteraction(Character other)
    {
        _preview.PreviewMove(other);
    }

    public void ClearInteractionPreview()
    {
        _preview.Clear();
    }

    // ── Attack Behaviour ─────────────────────────────────────────────────

    public IEnumerator GetMeleeSwipeSequence(Character other)
    {
        return _attackBehaviour.GetMeleeSwipeSequence(other);
    }

    // ── Hurt Behaviour ───────────────────────────────────────────────────

    public YieldInstruction PlayAttackConnectShake()
    {
        return _hurtBehaviour.PlayAttackConnectShake();
    }

    public IEnumerator GetHurtSequence(Character attacker)
    {
        return _hurtBehaviour.GetHurtSequence(attacker);
    }

    // ── Undo Redo Behaviour ──────────────────────────────────────────────

    public IEnumerator GetApplyStateSequence(CharacterState state)
    {
        return _undoRedoBehaviour.GetApplyStateSequence(state);
    }

    // ── Battle ───────────────────────────────────────────────────────────

    public Vector2 CellToWorld(Vector2Int cell)
    {
        return _battle.CellToWorld(cell);
    }

    public Vector2Int WorldToCell(Vector2 position)
    {
        return _battle.WorldToCell(position);
    }

    public Character GetOccupant(Vector2Int cell)
    {
        return _battle.GetOccupant(cell);
    }

    public void RefreshCell()
    {
        _battle.RefreshCell(this);
    }

    public TileBase GetTile(Vector2Int cell)
    {
        return _battle.GetTile(cell);
    }

    public void RecordMoveWith(Character other)
    {
        _battle.Record(this, other);
    }

    public void RecordMove()
    {
        _battle.Record(this);
    }

    public CharacterState GetLastRecordedState()
    {
        return _battle.GetLastRecordedState(this).State;
    }
}
