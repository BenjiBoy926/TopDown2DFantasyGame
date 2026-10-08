using UnityEngine;

[RequireComponent(typeof(Battle))]
public class BattleMusic : MonoBehaviour
{
    [SerializeField] private Music _playerTurn;
    [SerializeField] private Music _enemyTurn;
    private Battle _battle;

    private void Awake()
    {
        _battle = GetComponent<Battle>();
    }

    public void NotifyTurnChanged()
    {
        if (_battle.CurrentFactionTurn == _battle.PlayerFaction)
        {
            _playerTurn.Play();
            _enemyTurn.Stop();
        }
        else
        {
            _playerTurn.Stop();
            _enemyTurn.Play();
        }
    }
}