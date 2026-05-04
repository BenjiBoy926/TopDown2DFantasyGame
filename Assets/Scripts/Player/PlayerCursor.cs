using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    [SerializeField] private GameObject _attackSprite;
    [SerializeField] private GameObject _defendSprite;
    [SerializeField] private GameObject _healSprite;
    private Player _player;

    public void Refresh()
    {
        Character character = _player.GetCharacterAtCurrentCell();
        if (!character || character == _player.ActiveCharacter)
        {
            ShowDefault();
        }
        else if (character.Faction != _player.ActiveCharacterFaction)
        {
            ShowAttack();
        }
        else
        {
            ShowHeal();
        }
    }

    public void ShowAttack()
    {
        _attackSprite.SetActive(true);
        _defendSprite.SetActive(false);
        _healSprite.SetActive(false);
    }

    public void ShowDefend()
    {
        _attackSprite.SetActive(false);
        _defendSprite.SetActive(true);
        _healSprite.SetActive(false);
    }

    public void ShowHeal()
    {
        _attackSprite.SetActive(false);
        _defendSprite.SetActive(false);
        _healSprite.SetActive(true);
    }

    public void ShowDefault()
    {
        _attackSprite.SetActive(false);
        _defendSprite.SetActive(false);
        _healSprite.SetActive(false);
    }

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        ShowDefault();
    }
}