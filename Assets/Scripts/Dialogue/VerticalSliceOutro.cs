using System.Collections;
using UnityEngine;
using static UniqueCharacters;

public class VerticalSliceOutro : MonoBehaviour
{
    [SerializeField] private Overlay _fadeInOverlay;
    [SerializeField] private float _footstepWait = 1f;
    [SerializeField] private float _footstepStagger = .5f;
    private Battle _battle;
    private Player _player;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _player = _battle.GetComponentInChildren<Player>(true);
    }

    private IEnumerator Start()
    {
        SetupInitialState();
        yield return InitialFootstepFadeIn();
    }

    private void SetupInitialState()
    {
        _fadeInOverlay.SetAlpha(1);
    }

    private IEnumerator InitialFootstepFadeIn()
    {
        yield return new WaitForSeconds(_footstepWait);
        yield return StartRunning(Hayden, true);
        yield return StartRunning(Gregory, true);
        yield return StartRunning(Alfred, false);
        yield return StartRunning(Robin, false);

        _player.Show();
        yield return _fadeInOverlay.FadeOut();
    }

    private YieldInstruction StartRunning(Character character, bool audible)
    {
        character.SetFootstepsAudible(audible);
        character.SetIsRunning(true);
        return new WaitForSeconds(_footstepStagger);
    }
}