using UnityEngine;

[RequireComponent(typeof(Character))]
public class Commander : MonoBehaviour
{
    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _character.RegisterAsCommander();
    }

    private void OnDestroy()
    {
        _character.UnregisterAsCommander();
    }
}