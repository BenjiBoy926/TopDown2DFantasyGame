using System;
using System.Collections;
using UnityEngine;

public class BattleDialogue : MonoBehaviour
{
    public bool IsPlaying => _isPlaying;

    private BattleDialogueSequence[] _sequences;
    private bool _isPlaying;

    private void Awake()
    {
        _sequences = GetComponentsInChildren<BattleDialogueSequence>(true);
        Array.Sort(_sequences, BattleDialogueSequence.Comparison);
    }

    public IEnumerator GetMoveStartDialogue(Character interactor, Character target)
    {
        return GetDialogue(BattleDialogueSequence.TriggerType.MoveStart, interactor, target);
    }

    public IEnumerator GetMoveFinishDialogue(Character interactor, Character target)
    {
        return GetDialogue(BattleDialogueSequence.TriggerType.MoveEnd, interactor, target);
    }

    public IEnumerator GetTurnStartDialogue()
    {
        return GetDialogue(BattleDialogueSequence.TriggerType.TurnStart, null, null);
    }

    public IEnumerator GetUndoRedoEndDialogue()
    {
        return GetDialogue(BattleDialogueSequence.TriggerType.UndoRedoEnd, null, null);
    }

    private IEnumerator GetDialogue(BattleDialogueSequence.TriggerType trigger, Character interactor, Character target)
    {
        _isPlaying = true;
        foreach (var sequence in _sequences)
        {
            if (sequence.Trigger == trigger && sequence.ShouldPlay(interactor, target))
            {
                yield return sequence.Play(interactor, target);
            }
        }
        _isPlaying = false;
    }
}