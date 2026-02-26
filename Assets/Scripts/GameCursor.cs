using UnityEngine;

[RequireComponent(typeof(Player))]
public class GameCursor : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        Character.Clicked += OnCharacterClicked;
    }

    private void OnDisable()
    {
        Character.Clicked -= OnCharacterClicked;
    }

    private void OnCharacterClicked(Character obj)
    {
        _player.SetCharacter(obj);
    }
}
