using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAttackBehaviour : MonoBehaviour
{
    private static readonly WaitForSeconds _dealDamageWait = new(.1f);
    private static readonly WaitForSeconds _remainingAttackWait = new(.3f);
    private Character _character;

    public IEnumerator GetSequence(Character other)
    {
        _character.SecureCurrentCell();
        _character.LookAt(other.Position);

        _character.PlayAttackAnimation();

        yield return _dealDamageWait;
        other.TakeDamageFrom(_character);
        other.PlayHurtAnimation();
        
        yield return _remainingAttackWait;
        yield return _character.WaitInCurrentCell();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}