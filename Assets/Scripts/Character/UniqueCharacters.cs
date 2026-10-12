using System.Collections.Generic;
using UnityEngine;

public class UniqueCharacters : MonoBehaviour
{
    private const string HaydenName = "Hayden";
    private const string GregoryName = "Gregory";
    private const string AlfredName = "Alfred";
    private const string RobinName = "Robin";

    public static Character Hayden { get; private set; }
    public static Character Gregory { get; private set; }
    public static Character Alfred { get; private set; }
    public static Character Robin { get; private set; }
    public static List<Character> MainCharacters { get; private set; } = new() { null, null, null, null };

    public static void Register(Character character)
    {
        switch (character.Name)
        {
            case HaydenName:
                Hayden = character;
                MainCharacters[0] = character;
                break;
            case GregoryName:
                Gregory = character;
                MainCharacters[1] = character;
                break;
            case AlfredName:
                Alfred = character;
                MainCharacters[2] = character;
                break;
            case RobinName:
                Robin = character;
                MainCharacters[3] = character;
                break;
        }
    }
}