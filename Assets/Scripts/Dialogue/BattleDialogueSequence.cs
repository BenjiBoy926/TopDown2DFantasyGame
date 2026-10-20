using System;
using System.Collections;
using UnityEngine;

public abstract class BattleDialogueSequence : MonoBehaviour
{
    public enum TriggerType
    {
        MoveStart,
        MoveEnd,
        TurnStart,
        UndoRedoEnd
    }

    protected Battle Battle => _battle;
    protected bool HasPlayed => _timesPlayed > 0;
    public abstract TriggerType Trigger { get; }
    public virtual int Priority => 0;
    public static readonly Comparison<BattleDialogueSequence> Comparison = (a, b) => a.Priority.CompareTo(b.Priority);

    private Battle _battle;
    private int _timesPlayed = 0;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
    }

    public abstract bool ShouldPlay(Character interactor, Character target);
    public IEnumerator Play(Character interactor, Character target)
    {
        _timesPlayed++;
        return GetSequence(interactor, target);
    }
    protected abstract IEnumerator GetSequence(Character interactor, Character target);
}
