using System.Collections;
using UnityEngine;

public class VerticalSliceIntro : MonoBehaviour
{
    [SerializeField] private Character _alfred;

    private IEnumerator Start()
    {
        yield return _alfred.Say("Okay, I can't see a thing.");
    }
}