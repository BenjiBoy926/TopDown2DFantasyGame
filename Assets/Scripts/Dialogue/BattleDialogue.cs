using System.Collections;
using UnityEngine;

public class BattleDialogue : MonoBehaviour
{
    private BattleDialogueSequence[] _sequences;

    private void Awake()
    {
        _sequences = GetComponentsInChildren<BattleDialogueSequence>(true);
    }

    public IEnumerator GetMoveStartDialogue(Character interactor, Character target)
    {
        for (int i = 0; i < _sequences.Length; i++)
        {
            BattleDialogueSequence sequence = _sequences[i];
            if (sequence.ShouldPlayOnMoveStart(interactor, target))
            {
                yield return sequence.Play(interactor, target);
            }
        }
    }

    public IEnumerator GetMoveFinishDialogue(Character interactor, Character target)
    {
        for (int i = 0; i < _sequences.Length; i++)
        {
            BattleDialogueSequence sequence = _sequences[i];
            if (sequence.ShouldPlayOnMoveFinish(interactor, target))
            {
                yield return sequence.Play(interactor, target);
            }
        }
    }

    public IEnumerator GetTurnStartDialogue()
    {
        for (int i = 0; i < _sequences.Length; i++)
        {
            BattleDialogueSequence sequence = _sequences[i];
            if (sequence.ShouldPlayOnTurnStart())
            {
                yield return sequence.Play(null, null);
            }
        }
    }
}