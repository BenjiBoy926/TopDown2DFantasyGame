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
        other.LookAt(_character.Position);

        Coroutine attackAnimation = _character.PlayAttackAnimation();
        yield return _dealDamageWait;
        
        other.TakeDamageFrom(_character);
        yield return other.PlayHurtAnimation();
        other.PlayIdleAnimation();

        yield return attackAnimation;
        yield return _character.WaitInCurrentCell();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}