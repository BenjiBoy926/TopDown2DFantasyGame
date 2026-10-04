using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VerticalSliceIntro : MonoBehaviour
{
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private Character _alfred;
    private Dialogue _dialogue;
    private Battle _battle;

    private void Awake()
    {
        _dialogue = GetComponentInParent<Dialogue>();
        _battle = GetComponentInParent<Battle>();
    }

    private IEnumerator Start()
    {
        Color color = _globalLight.color;
        _globalLight.color = Color.black;

        yield return _dialogue.Say(_alfred, "Okay, I can't see a thing.");
        yield return new WaitForSeconds(2);

        _globalLight.color = color;
        _battle.Begin();
    }
}