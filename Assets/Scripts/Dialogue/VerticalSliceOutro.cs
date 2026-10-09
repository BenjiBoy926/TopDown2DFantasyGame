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
    private Vector2 _playerPosition;

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
        _playerPosition = _player.CursorPosition;
        _fadeInOverlay.SetAlpha(1);
    }

    private IEnumerator InitialFootstepFadeIn()
    {
        yield return new WaitForSeconds(_footstepWait);
        Hayden.SetIsRunning(true);
        yield return new WaitForSeconds(_footstepStagger);
        Gregory.SetFootstepsAudible(false);
        Gregory.SetIsRunning(true);
        yield return new WaitForSeconds(_footstepStagger);
        Alfred.SetFootstepsAudible(false);
        Alfred.SetIsRunning(true);
        yield return new WaitForSeconds(_footstepStagger);
        Robin.SetFootstepsAudible(false);
        Robin.SetIsRunning(true);

        _player.SetPosition(_playerPosition);
        yield return _fadeInOverlay.FadeOut();
        _player.Show();
    }
}