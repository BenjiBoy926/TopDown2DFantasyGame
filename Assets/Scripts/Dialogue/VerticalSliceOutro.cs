using System.Collections;
using UnityEngine;
using static UniqueCharacters;

public class VerticalSliceOutro : MonoBehaviour
{
    [SerializeField] private Overlay _fadeInOverlay;
    [SerializeField] private float _footstepWait = 1f;
    [SerializeField] private float _footstepStagger = .5f;
    [SerializeField] private float _walkSpeed;
    private Battle _battle;
    private Player _player;
    private Coroutine _walkingRoutine;
    private Vector2 _initialWalkPosition;
    private Vector2 _finalWalkPosition;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _player = _battle.GetComponentInChildren<Player>(true);
        _initialWalkPosition = transform.position;
        _finalWalkPosition = _initialWalkPosition + (Vector2.right * _walkSpeed);
    }

    private IEnumerator Start()
    {
        SetupInitialState();
        yield return InitialFootstepFadeIn();
    }

    private void SetupInitialState()
    {
        _fadeInOverlay.SetAlpha(1);
        _player.HideGridReticle();
    }

    private IEnumerator InitialFootstepFadeIn()
    {
        yield return new WaitForSeconds(_footstepWait);
        yield return StartRunning(Hayden, true);
        yield return StartRunning(Gregory, true);
        yield return StartRunning(Alfred, false);
        yield return StartRunning(Robin, false);

        _walkingRoutine = StartCoroutine(WalkingRoutine());
        _player.ShowCursor();
        yield return _fadeInOverlay.FadeOut();
    }

    private YieldInstruction StartRunning(Character character, bool audible)
    {
        character.SetFootstepsAudible(audible);
        character.SetIsRunning(true);
        return new WaitForSeconds(_footstepStagger);
    }

    private IEnumerator WalkingRoutine()
    {
        float t = 0;

        while (true)
        {
            t += Time.deltaTime;
            t = Mathf.Repeat(t, 1);
            Vector2 position = Vector2.LerpUnclamped(_initialWalkPosition, _finalWalkPosition, t);
            SetPosition(position);
            yield return null;
        }
    }

    private void SetPosition(Vector2 position)
    {
        transform.position = position;
        _player.SetPosition(position + Vector2.right * 2);
        Hayden.Position = position;
        Gregory.Position = position + Vector2.left;
        Alfred.Position = position + Vector2.left * 2;
        Robin.Position = position + Vector2.left * 3;
        _battle.SetCameraTransformPosition(position);
    }
}