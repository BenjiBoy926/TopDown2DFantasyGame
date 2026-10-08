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
    [SerializeField] private AudioClip _ambushMusic;
    [SerializeField] private float _ambushMusicVolume = .2f;
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private float _initialEmberRevealDelay = 2;
    [SerializeField] private float _emberPulseDuration = 3;
    [SerializeField] private float _emberFadeInDuration = 4;
    [SerializeField] private float _globalLightFadeDelay = 1;
    [SerializeField] private float _globalLightFadeDuration = 4;
    private Battle _battle;
    private Player _player;
    private PlayerCursor _cursor;
    private Light2D _ember;
    private Vector2 _playerPosition;
    private Color _globalLightColor;
    private float _emberIntensity;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _player = _battle.GetComponentInChildren<Player>(true);
        _cursor = _battle.GetComponentInChildren<PlayerCursor>(true);
        _ember = _cursor.GetComponentInChildren<Light2D>(true);
    }

    private IEnumerator Start()
    {
        _playerPosition = _player.CursorPosition;
        _globalLightColor = _globalLight.color;
        _emberIntensity = _ember.intensity;

        _globalLight.color = Color.black;
        _ember.intensity = 0;

        yield return SoundFadeInSequence();

        yield return Alfred.Say("Okay, I can't see a thing.");
        yield return Gregory.Say("How much further, Hayden?");
        yield return Hayden.Say("It's just ahead. I'm sure of it.");
        yield return Alfred.Say("I'm bored. If nothing happens soon, I'm turning back.");
        yield return Gregory.Say("Nonesense! We must stick together!");
        yield return Hayden.Say("Shh!");

        yield return LightFadeInSequence();

        yield return Hayden.Say("There! It's the Ember! Oh, thank Arkalux!");
        int ambushMusicID = EazySoundManager.PlayMusic(_ambushMusic, _ambushMusicVolume, true, true);
        yield return Robin.Say("Don't thank him just yet. It's led us into a trap.");
        yield return Hayden.Say("What?");
        yield return Robin.Say("Goblins ahead. Four. Several more behind them.");
        yield return Alfred.Say("We can take them!");
        yield return Robin.Say("Don't be ridiculous. We have to retreat.");
        yield return Hayden.Say("No! We'll only be lost in the dark.");
        yield return Hayden.Say("We must stay near the Ember at all costs.");
        yield return Gregory.Say("He's right. If we work together, we can win. I'm sure of it.");
        Audio ambushAudio = EazySoundManager.GetAudio(ambushMusicID);
        ambushAudio.Stop();

        _battle.Begin();
    }

    private IEnumerator SoundFadeInSequence()
    {
        WaitForSeconds waitBeforeStart = new(1);
        WaitForSeconds waitBetweenSteps = new(.07f);
        WaitForSeconds waitBeforeEnd = new(1);

        EazySoundManager.PlaySound(_ambience, _ambienceVolume, true, null);
        yield return waitBeforeStart;
        Hayden.SetIsRunning(true);
        yield return waitBetweenSteps;
        Gregory.SetIsRunning(true);
        yield return waitBeforeEnd;
    }

    private IEnumerator LightFadeInSequence()
    {
        yield return new WaitForSeconds(_initialEmberRevealDelay);
        Hayden.PlayIdleAnimation();
        Gregory.PlayIdleAnimation();

        _cursor.Show();
        _player.SetPosition(_playerPosition);
        
        Vector3 GetEmberIntensity() => new(_ember.intensity, 0, 0);
        void SetEmberIntensity(Vector3 value) => _ember.intensity = value.x;
        Vector3 emberDirection = Vector3.right * .5f;

        yield return DOTween.Punch(GetEmberIntensity, SetEmberIntensity, emberDirection, _emberPulseDuration, 0).WaitForCompletion();
        yield return DOTween.Punch(GetEmberIntensity, SetEmberIntensity, emberDirection, _emberPulseDuration, 0).WaitForCompletion();
        yield return DOTween.To(GetEmberIntensity, SetEmberIntensity, Vector3.right, _emberFadeInDuration).WaitForCompletion();

        yield return new WaitForSeconds(_globalLightFadeDelay);

        Color GetLightColor() => _globalLight.color;
        void SetLightColor(Color color) => _globalLight.color = color;
        yield return DOTween.To(GetLightColor, SetLightColor, _globalLightColor, _globalLightFadeDuration).WaitForCompletion();
    }
}