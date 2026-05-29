using DG.Tweening;
using Hellmade.Sound;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterDefendBehaviour : MonoBehaviour
{
    [SerializeField] private float _moveDuration = .35f;
    [SerializeField] private Ease _moveEase = Ease.OutCubic;
    [SerializeField] private AudioClip _clip;
    private Character _character;

    public IEnumerator GetSequence()
    {
        EazySoundManager.PlaySound(_clip);

        _character.ConfirmMove();

        Vector2 targetPosition = _character.CurrentCellCenter;
        _character.SetIsRunning(true);
        yield return transform.DOMove(targetPosition, _moveDuration)
            .SetEase(_moveEase)
            .WaitForCompletion();
        _character.SetIsRunning(false);

        _character.RecordMove();
        yield return _character.MoveFadeOut();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}