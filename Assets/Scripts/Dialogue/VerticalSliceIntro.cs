using DG.Tweening;
using Hellmade.Sound;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UniqueCharacters;

public class VerticalSliceIntro : MonoBehaviour
{
    [SerializeField] private AudioClip _ambience;
    [SerializeField] private float _ambienceVolume = 1;
    [SerializeField] private float _waitBeforeFootstepStart = 2;
    [SerializeField] private float _footstepStagger = .15f;
    [SerializeField] private float _waitBeforeTalkingStarts = 1;

    [Space]
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private float _footstepStopDelay = 1;
    [SerializeField] private float _initialEmberRevealDelay = 2;
    [SerializeField] private float _emberPulseDuration = 3;
    [SerializeField] private float _emberFadeInDuration = 4;
    [SerializeField] private float _globalLightFadeDelay = 1;
    [SerializeField] private float _globalLightFadeDuration = 4;

    [Space]
    [SerializeField] private Music _ambush;

    private Battle _battle;
    private Player _player;
    private Light2D _ember;
    private Color _globalLightColor;
    private float _emberIntensity;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _player = _battle.GetComponentInChildren<Player>(true);
        _ember = _player.GetComponentInChildren<Light2D>(true);
    }

    private IEnumerator Start()
    {
        SetupInitialState();

        yield return SoundFadeInSequence();

        yield return Alfred.Say("Okay, I can't see a thing.");
        yield return Gregory.Say("How much further, Hayden?");
        yield return Hayden.Say("It's just ahead. I'm sure of it.");
        yield return Alfred.Say("I'm bored. If nothing happens soon, I'm turning back.");
        yield return Gregory.Say("Nonesense! We must stick together!");
        yield return Hayden.Say("Shh!");

        yield return LightFadeInSequence();

        yield return Hayden.Say("There! It's the Ember! Oh, thank Arkalux!");
        _ambush.Play();
        yield return Robin.Say("Don't thank him just yet. It's led us into a trap.");
        yield return Hayden.Say("What?");
        yield return Robin.Say("Goblins ahead. Four. Several more behind them.");
        yield return Alfred.Say("FINALLY! Come on! We can take 'em!");
        yield return Robin.Say("Don't be ridiculous. We have to retreat.");
        yield return Hayden.Say("No! We'll only be lost in the dark.");
        yield return Hayden.Say("We must stay near the Ember at all costs.");
        yield return Gregory.Say("He's right. If we work together, we can win. I'm sure of it.");
        _ambush.Stop();

        _battle.Begin();
    }

    private void SetupInitialState()
    {
        _globalLightColor = _globalLight.color;
        _emberIntensity = _ember.intensity;

        _globalLight.color = Color.black;
        _ember.intensity = 0;
    }

    private IEnumerator SoundFadeInSequence()
    {
        int ambienceID = EazySoundManager.PlaySound(_ambience, 1, true, null);
        yield return new WaitForSeconds(_waitBeforeFootstepStart);
        Audio ambience = EazySoundManager.GetAudio(ambienceID);
        ambience.SetVolume(_ambienceVolume);

        Hayden.SetIsRunning(true);
        yield return new WaitForSeconds(_footstepStagger);
        Gregory.SetIsRunning(true);
        yield return new WaitForSeconds(_waitBeforeTalkingStarts);
    }

    private IEnumerator LightFadeInSequence()
    {
        yield return new WaitForSeconds(_footstepStopDelay);
        Hayden.PlayIdleAnimation();
        Gregory.PlayIdleAnimation();

        yield return new WaitForSeconds(_initialEmberRevealDelay);
        _player.ShowCursor();
        
        Vector3 GetEmberIntensity() => new(_ember.intensity, 0, 0);
        void SetEmberIntensity(Vector3 value) => _ember.intensity = value.x;
        Vector3 emberDirection = Vector3.right * .5f;

        yield return DOTween.Punch(GetEmberIntensity, SetEmberIntensity, emberDirection, _emberPulseDuration, 0).WaitForCompletion();
        yield return DOTween.Punch(GetEmberIntensity, SetEmberIntensity, emberDirection, _emberPulseDuration, 0).WaitForCompletion();
        yield return DOTween.To(GetEmberIntensity, SetEmberIntensity, Vector3.right * _emberIntensity, _emberFadeInDuration).WaitForCompletion();

        yield return new WaitForSeconds(_globalLightFadeDelay);

        Color GetLightColor() => _globalLight.color;
        void SetLightColor(Color color) => _globalLight.color = color;
        yield return DOTween.To(GetLightColor, SetLightColor, _globalLightColor, _globalLightFadeDuration).WaitForCompletion();
    }
}