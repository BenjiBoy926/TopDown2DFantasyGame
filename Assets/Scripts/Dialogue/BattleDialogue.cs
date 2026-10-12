using System.Collections;
using UnityEngine;

public class BattleDialogue : MonoBehaviour
{
    private BattleDialogueSequence[] _sequences;

    private void Awake()
    {
        _sequences = GetComponentsInChildren<BattleDialogueSequence>(true);
    }

    public IEnumerator GetDialogue(Character interactor, Character target)
    {
        for (int i = 0; i < _sequences.Length; i++)
        {
            BattleDialogueSequence sequence = _sequences[i];
            if (sequence.ShouldPlay(interactor, target))
            {
                yield return sequence.Play(interactor, target);
            }
        }
    }
}