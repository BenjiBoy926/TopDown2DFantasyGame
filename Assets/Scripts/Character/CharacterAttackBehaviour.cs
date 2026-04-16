using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAttackBehaviour : MonoBehaviour
{
    private static readonly WaitForSeconds _dealDamageWait = new(.1f);
    private Character _character;

    public IEnumerator GetSequence(Character other)
    {
        _character.SecureCurrentCell();
        _character.LookAt(other.Position);

        Coroutine attackRoutine = _character.PlayAttackAnimation();
        yield return OtherFlinchSequence(other);
        yield return attackRoutine;
        yield return _character.WaitInCurrentCell();
    }

    private IEnumerator OtherFlinchSequence(Character other)
    {
        other.LookAt(_character.Position);
        yield return _dealDamageWait;

        other.TakeDamageFrom(_character);
        yield return other.PlayHurtAnimation();
        if (other.IsDead)
        {
            yield return other.PlayDieAnimation();
        }
        else
        {
            other.PlayIdleAnimation();
        }
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}