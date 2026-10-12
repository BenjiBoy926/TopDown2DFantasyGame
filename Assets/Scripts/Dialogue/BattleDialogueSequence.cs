using System.Collections;
using UnityEngine;

public abstract class BattleDialogueSequence : MonoBehaviour
{
    protected Battle Battle => _battle;
    protected bool HasPlayed => _timesPlayed > 0;

    private Battle _battle;
    private int _timesPlayed = 0;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
    }

    public abstract bool ShouldPlayOnMoveStart(Character interactor, Character target);
    public abstract bool ShouldPlayOnMoveFinish(Character interactor, Character target);
    public virtual IEnumerator Play(Character interactor, Character target)
    {
        _timesPlayed++;
        yield return null;
    }
}
