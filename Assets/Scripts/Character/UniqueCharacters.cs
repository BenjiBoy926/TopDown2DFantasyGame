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

    public static void Register(Character character)
    {
        switch (character.Name)
        {
            case HaydenName:
                Hayden = character;
                break;
            case GregoryName:
                Gregory = character;
                break;
            case AlfredName:
                Alfred = character;
                break;
            case RobinName:
                Robin = character;
                break;
        }
    }
}