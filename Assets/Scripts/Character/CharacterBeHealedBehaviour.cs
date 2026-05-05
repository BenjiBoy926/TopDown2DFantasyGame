using UnityEngine;
using System.Collections;
using DG.Tweening;
using Hellmade.Sound;

[RequireComponent(typeof(Character))]
public class CharacterBeHealedBehaviour : MonoBehaviour
{
    [SerializeField] private float _hopHeight = 0.3f;
    [SerializeField] private float _hopDuration = 0.7f;
    [SerializeField] private int _vibrato = 10;
    [SerializeField] private float _elasticity = 0;

    [Space]
    [SerializeField] private AudioClip _healSound;

    private Character _character;

    public IEnumerator GetSequence()
    {

        // TODO: slight change to animation if they are dead and being revived instead of just healed

        _character.RestoreHealth();
        EazySoundManager.PlaySound(_healSound);
        Vector2 punch = _character.CellHeight * _hopHeight * Vector2.up;
        yield return transform.DOPunchPosition(punch, _hopDuration, _vibrato, _elasticity).WaitForCompletion();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}