using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAttackBehaviour : MonoBehaviour
{
    [SerializeField] private float _animationFrameDuration = 0.1f;
    private Character _character;

    public void Attack(Character other)
    {
        Vector2 thisPosition = _character.CellToWorld(_character.CurrentCell);
        Vector2 otherPosition = other.Position;
        _character.SetDirection(otherPosition - thisPosition);
        _character.PlayAttackAnimation();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}