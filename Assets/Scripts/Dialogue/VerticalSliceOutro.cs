using DG.Tweening;
using System.Collections;
using UnityEngine;
using static UniqueCharacters;

public class VerticalSliceOutro : MonoBehaviour
{
    [SerializeField] private Overlay _fadeInOverlay;
    [SerializeField] private float _footstepWait = 1f;
    [SerializeField] private float _footstepStagger = .5f;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _haydenPause = 2;
    [SerializeField] private float _haydenPause2 = 2;
    private Battle _battle;
    private Player _player;
    private Vector2 _initialWalkPosition;
    private Vector2 _finalWalkPosition;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _player = _battle.GetComponentInChildren<Player>(true);
        _initialWalkPosition = transform.position;
        _finalWalkPosition = _initialWalkPosition + Vector2.right;
    }

    private IEnumerator Start()
    {
        SetupInitialState();
        yield return InitialFootstepFadeIn();

        yield return Alfred.Say("That was AMAZING!");
        yield return Alfred.Say("An entire battalion of goblins, and there's hardly a scratch on us!");
        yield return Robin.Say("This 'Ember' has unusual healing powers.");
        yield return Robin.Say("It seems like it can even manipulate time...");
        yield return Alfred.Say("Woah, woah, what? Time travel? What are you talking about?");
        yield return Robin.Say("You saw it too, didn't you?");
        yield return Robin.Say("One moment you're in one part of the battlefield, then there's a bright flash, and suddenly you're back where you started.");
        yield return Alfred.Say("You think that was the Ember... turning back time?!");
        yield return Alfred.Say("We're UNSTOPPABLE!!");
        yield return Gregory.Say("Not so fast.");
        yield return Gregory.Say("It can only heal through one of us.");
        yield return Gregory.Say("If we all die, there's no coming back from that.");
        yield return Alfred.Say("But then the Ember would just rewind time so we could change strategies, right?");
        yield return Gregory.Say("I wouldn't be too sure...");
        yield return Gregory.Say("Hayden, what do you think?");
        yield return new WaitForSeconds(_haydenPause);
        yield return Gregory.Say("Hayden?");
        yield return Hayden.Say("Huh? Oh, sorry, I wasn't listening. What were you saying?");
        yield return Gregory.Say("Are you alright? You haven't said a word since we started marching.");
        yield return Hayden.Say("Oh, yes. I'm alright.");
        yield return Hayden.Say("Hey, what do you think 'Ha'all' is?");
        yield return Gregory.Say("Ha'all?");
        yield return Robin.Say("The word the goblins were repeating during the battle.");
        yield return Alfred.Say("You can understand their language?!");
        yield return Robin.Say("They're speaking OUR language you know, just through obnoxious grunts.");
        yield return Robin.Say("Kind of like someone else I know...");
        yield return Alfred.Say("What's that supposed to mean?!");
        yield return Hayden.Say("Aaaaaaanyways!");
        yield return Hayden.Say("Ha'all? I'm not familiar with the word from any ancient lore.");
        yield return Robin.Say("They said the word with reverence. I think it might be a name.");
        yield return Hayden.Say("A name?");
        yield return Robin.Say("Today marks the first Orc sighting in a thousand years.");
        yield return Robin.Say("Perhaps Ha'all is some great Orc leading them out of hiding.");
        yield return Hayden.Say("How strange...");
        yield return new WaitForSeconds(_haydenPause2);
        yield return Hayden.Say("(Who is Ha'all? And where could the Ember be leading us?)");
        yield return Hayden.Say("(What could it all mean...?)");

        transform.DOKill();
        yield return _fadeInOverlay.FadeIn();
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

        transform.DOMove(_finalWalkPosition, _walkSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .OnUpdate(UpdateWalkFollowers);
        _player.ShowCursor();
        yield return _fadeInOverlay.FadeOut();
    }

    private YieldInstruction StartRunning(Character character, bool audible)
    {
        character.SetFootstepsAudible(audible);
        character.SetIsRunning(true);
        return new WaitForSeconds(_footstepStagger);
    }

    private void UpdateWalkFollowers()
    {
        Vector2 position = transform.position;
        _battle.CameraPosition = position;
        _player.SetPosition(position + Vector2.right * 2);
        Hayden.Position = position;
        Gregory.Position = position + Vector2.left;
        Alfred.Position = position + Vector2.left * 2;
        Robin.Position = position + Vector2.left * 3;
    }
}