using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Character))]
public class CharacterBeHealedBehaviour : MonoBehaviour
{
    [SerializeField] private float _initialDelay = 0.3f;
    [SerializeField] private float _hopHeight = 0.3f;
    [SerializeField] private float _hopDuration = 0.3f;
    [SerializeField] private Ease _hopUpEase = Ease.OutQuad;
    [SerializeField] private Ease _fallBackEase = Ease.OutBounce;

    private Character _character;

    public IEnumerator GetSequence()
    {
        WaitForSeconds initialWait = new(_initialDelay);
        yield return initialWait;

        _character.RestoreHealth();

        // TODO: slight change to animation if they are dead and being revived instead of just healed

        Vector2 hopPeak = _character.CurrentCellCenter + _hopHeight * _character.CellHeight * Vector2.up;
        yield return transform.DOMove(hopPeak, _hopDuration)
            .SetEase(_hopUpEase)
            .WaitForCompletion();
        yield return transform.DOMove(_character.CurrentCellCenter, _hopDuration)
            .SetEase(_fallBackEase)
            .WaitForCompletion();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
}