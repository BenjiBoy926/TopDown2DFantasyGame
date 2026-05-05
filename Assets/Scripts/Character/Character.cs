using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(CharacterRange))]
[RequireComponent(typeof(CharacterAttackBehaviour))]
[RequireComponent(typeof(CharacterHurtBehaviour))]
[RequireComponent(typeof(CharacterDefendBehaviour))]
[RequireComponent(typeof(CharacterHealBehaviour))]
[RequireComponent(typeof(CharacterBeHealedBehaviour))]
[RequireComponent(typeof(CharacterCancelBehaviour))]
[RequireComponent(typeof(CharacterStats))]
public class Character : MonoBehaviour
{
    public static event Action<Character> MoveFinished = delegate { };

    public IReadOnlyCollection<Vector2Int> StayableCells => _range.StayableCells;
    public IReadOnlyCollection<Vector2Int> InteractableEdgeCells => _range.InteractableEdgeCells;
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    public Vector2Int HomeCell => _battle.GetCell(this);
    public Vector2Int CurrentCell => _battle.WorldToCell(Position);
    public Vector2 CurrentCellCenter => _battle.SnapToGrid(Position);
    public Faction Faction => _faction;
    public bool IsAbleToMove => !IsDead && !_hasMovedThisTurn;
    public float CellWidth => _battle.CellWidth;
    public float CellHeight => _battle.CellHeight;
    public int Power => _stats.Power;
    public bool CanBeRevived => _faction.CanBeRevived;
    public bool IsDead => _stats.IsDead;
    public Vector2 CellSize => new(_battle.CellWidth, _battle.CellHeight);
    public int TraversalRange => _stats.TraversalRange;

    [SerializeField] private Faction _faction;
    [SerializeField] private Color _usedMoveFadeColor = Color.gray;
    [SerializeField] private float _usedMoveFadeDuration = 0.35f;
    private CharacterAnimator _animator;
    private SpriteRenderer _sprite;
    private CharacterRange _range;
    private CharacterAttackBehaviour _attackBehaviour;
    private CharacterHurtBehaviour _hurtBehaviour;
    private CharacterDefendBehaviour _defendBehaviour;
    private CharacterHealBehaviour _healBehaviour;
    private CharacterBeHealedBehaviour _beHealedBehaviour;
    private CharacterCancelBehaviour _cancelBehaviour;
    private CharacterStats _stats;
    private Battle _battle;
    private bool _hasMovedThisTurn = false;

    public void LookAt(Vector2 position)
    {
        Vector2 direction = position - Position;
        SetDirection(direction);
    }

    public void SetDirection(Vector2 direction)
    {
        _animator.SetDirection(direction);
    }

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

    public YieldInstruction PlayAttackConnectShake()
    {
        return _hurtBehaviour.PlayAttackConnectShake();
    }

    public void PlayIdleAnimation()
    {
        SetIsRunning(false);
        _animator.PlayLoopingAnimation();
    }

    public YieldInstruction FadeAlpha(float alpha, float duration, Ease ease)
    {
        return _sprite.DOFade(alpha, duration).SetEase(ease).WaitForCompletion();
    }

    public void SetIsRunning(bool isRunning)
    {
        _animator.SetIsRunning(isRunning);
    }

    public Coroutine Attack(Character other)
    {
        StopAllCoroutines();
        IEnumerator sequence = _attackBehaviour.GetAttackSequence(other);
        return StartCoroutine(sequence);
    }

    public Coroutine Hurt(Character other)
    {
        StopAllCoroutines();
        IEnumerator sequence = _hurtBehaviour.GetHurtSequence(other);
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

    public void SecureCurrentCell()
    {
        SetHasMovedThisTurn(true);
        _battle.RefreshCell(this);
    }

    public void RestoreMove()
    {
        SetHasMovedThisTurn(false);
        PerformSpriteFade();
    }

    public Vector2 ClampToReachableCells(Vector2 input)
    {
        return _range.ClampToReachableCells(input);
    }

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

    public Vector2 ClampToTraversibleCells(Vector2 position)
    {
        return _range.ClampToStayableCells(position);
    }

    public bool CanStayInCell(Vector2Int cell)
    {
        Character occupant = GetOccupant(cell);
        return !occupant || occupant == this;
    }

    public IEnumerator MoveFadeOut()
    {
        yield return PerformSpriteFade();
        MoveFinished.Invoke(this);
    }

    public void TakeDamageFrom(Character other)
    {
        _stats.TakeDamageFrom(other);
    }

    public void RestoreHealth()
    {
        _stats.RestoreHealth();
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<CharacterAnimator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _range = GetComponent<CharacterRange>();
        _attackBehaviour = GetComponent<CharacterAttackBehaviour>();
        _hurtBehaviour = GetComponent<CharacterHurtBehaviour>();
        _defendBehaviour = GetComponent<CharacterDefendBehaviour>();
        _healBehaviour = GetComponent<CharacterHealBehaviour>();
        _beHealedBehaviour = GetComponent<CharacterBeHealedBehaviour>();
        _cancelBehaviour = GetComponent<CharacterCancelBehaviour>();
        _stats = GetComponent<CharacterStats>();
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
    }

    private void Start()
    {
        _battle = GetComponentInParent<Battle>();
        if (_battle)
        {
            _battle.Register(this);
        }
    }
    
    private void SetHasMovedThisTurn(bool hasMovedThisTurn)
    {
        _hasMovedThisTurn = hasMovedThisTurn;
    }

    private YieldInstruction PerformSpriteFade()
    {
        _sprite.DOKill();
        if (_hasMovedThisTurn)
        {
            return _sprite.DOColor(_usedMoveFadeColor, _usedMoveFadeDuration).WaitForCompletion();
        }
        else
        {
            return _sprite.DOColor(Color.white, _usedMoveFadeDuration).WaitForCompletion();
        }
    }
}
