using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UniqueCharacters;

public class VerticalSliceIntro : MonoBehaviour
{
    [SerializeField] private Light2D _globalLight;
    private Battle _battle;

    private void Awake()
    {
        _battle = GetComponentInParent<Battle>();
    }

    private IEnumerator Start()
    {
        Color color = _globalLight.color;
        _globalLight.color = Color.black;

        yield return Alfred.Say("Okay, I can't see a thing.");
        yield return Gregory.Say("How much further, Hayden?");
        yield return Hayden.Say("It's just ahead. I'm sure of it.");
        yield return Alfred.Say("I'm bored. If nothing happens soon, I'm turning back.");
        yield return Gregory.Say("Nonesense! We must stick together!");
        yield return Hayden.Say("Shh!");

        yield return LightFadeInSequence(color);

        yield return Hayden.Say("There! It's the Ember! Oh, thank Arkalux!");
        yield return Robin.Say("Don't thank him just yet. It's led us into a trap.");
        yield return Hayden.Say("What?");
        yield return Robin.Say("Goblins ahead. Four. Several more behind them.");
        yield return Alfred.Say("We can take them!");
        yield return Robin.Say("Don't be rediculous. We have to retreat.");
        yield return Hayden.Say("No! We'll only be lost in the dark. We have to stay near the Ember, it's our only hope.");
        yield return Gregory.Say("He's right. If we work together, we can win. I'm sure of it.");

        _battle.Begin();
    }

    private IEnumerator LightFadeInSequence(Color formerLightColor)
    {
        yield return new WaitForSeconds(2f);
        _globalLight.color = formerLightColor;
    }
}