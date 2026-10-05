using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

        yield return UniqueCharacters.Alfred.Say("Okay, I can't see a thing.");
        yield return new WaitForSeconds(2);

        _globalLight.color = color;
        _battle.Begin();
    }
}