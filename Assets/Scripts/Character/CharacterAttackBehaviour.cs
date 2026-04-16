using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAttackBehaviour : MonoBehaviour
{
    private static WaitForSeconds _dealDamageWait = new WaitForSeconds(.1f);
    private Character _character;

    public IEnumerator GetSequence(Character other)
    {
        _character.SecureCurrentCell();
        _character.LookAt(other.Position);

        Coroutine attackAnimation = _character.PlayAttackAnimation();

        yield return _dealDamageWait;
        other.TakeDamageFrom(_character);
        other.PlayHurtAnimation();
        
        yield return attackAnimation;
        yield return _character.WaitInCurrentCell();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}