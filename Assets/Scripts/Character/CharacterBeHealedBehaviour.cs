using UnityEngine;
using System.Collections;
using DG.Tweening;
using Hellmade.Sound;

[RequireComponent(typeof(Character))]
public class CharacterBeHealedBehaviour : MonoBehaviour
{
    [SerializeField] private float _reviveTwitchDuration = .3f;
    [SerializeField] private float _reviveTwitchMagnitude = .1f;
    [SerializeField] private int _reviveTwitchVibrato = 30;
    [SerializeField] private float _reviveTwitchElastiticy = 1;
    [SerializeField] private float _reviveTwitchPause = 0.5f;

    [Space]
    [SerializeField] private float _hopDuration = 0.7f;
    [SerializeField] private float _hopHeight = 0.49f;
    [SerializeField] private int _hopVibrato = 10;
    [SerializeField] private float _hopElasticity = 0;

    [Space]
    [SerializeField] private AudioClip _healSound;

    private Character _character;

    public IEnumerator GetSequence(Character other)
    {
        // TODO: slight change to animation if they are dead and being revived instead of just healed

        if (_character.IsDead)
        {
            yield return GetReviveTwitchSequence(other);
        }

        _character.RestoreHealth();
        EazySoundManager.PlaySound(_healSound);

        Vector2 punch = _character.CellHeight * _hopHeight * Vector2.up;
        yield return transform.DOPunchPosition(punch, _hopDuration, _hopVibrato, _hopElasticity).WaitForCompletion();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private IEnumerator GetReviveTwitchSequence(Character other)
    {
        Vector3 punch = _character.CellWidth * _reviveTwitchMagnitude * Vector3.right;
        yield return transform.DOPunchPosition(punch, _reviveTwitchDuration, _reviveTwitchVibrato, _reviveTwitchElastiticy).WaitForCompletion();

        WaitForSeconds wait = new(_reviveTwitchPause);
        yield return wait;

        _character.PlayIdleAnimation();
        _character.LookAt(other.Position);
    }
}