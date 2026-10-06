using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UniqueCharacters;

public class VerticalSliceIntro : MonoBehaviour
{
    [SerializeField] private Light2D _globalLight;
    private Battle _battle;
    private PlayerCursor _cursor;
    private Light2D _ember;
    private Color _globalLightColor;
    private float _emberIntensity;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
        _cursor = _battle.GetComponentInChildren<PlayerCursor>(true);
        _ember = _cursor.GetComponentInChildren<Light2D>(true);

        _globalLightColor = _globalLight.color;
        _emberIntensity = _ember.intensity;

        _globalLight.color = Color.black;
        _ember.intensity = 0;
    }

    private IEnumerator Start()
    {
        yield return Alfred.Say("Okay, I can't see a thing.");
        yield return Gregory.Say("How much further, Hayden?");
        yield return Hayden.Say("It's just ahead. I'm sure of it.");
        yield return Alfred.Say("I'm bored. If nothing happens soon, I'm turning back.");
        yield return Gregory.Say("Nonesense! We must stick together!");
        yield return Hayden.Say("Shh!");

        yield return LightFadeInSequence();

        yield return Hayden.Say("There! It's the Ember! Oh, thank Arkalux!");
        yield return Robin.Say("Don't thank him just yet. It's led us into a trap.");
        yield return Hayden.Say("What?");
        yield return Robin.Say("Goblins ahead. Four. Several more behind them.");
        yield return Alfred.Say("We can take them!");
        yield return Robin.Say("Don't be ridiculous. We have to retreat.");
        yield return Hayden.Say("No! We'll only be lost in the dark.");
        yield return Hayden.Say("We must stay near the Ember at all costs.");
        yield return Gregory.Say("He's right. If we work together, we can win. I'm sure of it.");

        _battle.Begin();
    }

    private IEnumerator LightFadeInSequence()
    {
        yield return new WaitForSeconds(2f);
        _globalLight.color = _globalLightColor;
        _ember.intensity = _emberIntensity;
    }
}