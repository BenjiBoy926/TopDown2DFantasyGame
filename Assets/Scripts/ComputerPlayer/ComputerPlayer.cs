using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Battle))]
public class ComputerPlayer : MonoBehaviour
{
    [SerializeField] private float _initialStartDelay = 1f;
    private Battle _battle;
    private Player _player;

    private void Awake()
    {
        _battle = GetComponent<Battle>();
        _player = GetComponentInChildren<Player>();
    }

    private void OnEnable()
    {
        BattleTurn.NextTurnStarted += OnNextTurnStarted;
    }

    private void OnDisable()
    {
        BattleTurn.NextTurnStarted -= OnNextTurnStarted;
    }

    private void OnNextTurnStarted(Faction faction)
    {
        if (faction != _player.Faction)
        {
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        while (_battle.IsTurnChangeAnimationPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(_initialStartDelay);
        _battle.StartNextTurn(); //lol
    }
}
