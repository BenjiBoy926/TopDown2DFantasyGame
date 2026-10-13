using System.Collections;
using static UniqueCharacters;

public class Dialogue
{
    public static IEnumerator MainCharacterSay(Character character, params string[] lines)
    {
        for (int i = 0; i < MainCharacters.Count; i++)
        {
            Character mainCharacter = MainCharacters[i];
            if (character == mainCharacter && i < lines.Length)
            {
                return mainCharacter.Say(lines[i]);
            }
        }
        return null;
    }
}