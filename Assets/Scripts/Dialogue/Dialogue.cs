using System;
using System.Collections;
using System.Collections.Generic;
using static UniqueCharacters;

public class Dialogue
{
    public readonly struct Entry
    {
        public Character Character => _character;
        public string Line => _line;

        private readonly Character _character;
        private readonly string _line;

        public Entry(Character character, string line)
        {
            _character = character;
            _line = line;
        }

        public IEnumerator Say()
        {
            return _character.Say(_line);
        }
    }

    public static IEnumerator FirstMatchingMainCharacterSay(Character character, params string[] lines)
    {
        Entry[] entries = new Entry[lines.Length];
        for (int i = 0; i < lines.Length && i < MainCharacters.Count; i++)
        {
            entries[i] = new(MainCharacters[i], lines[i]);
        }
        return FirstMatchingCharacterSay(character, entries);
    }

    public static IEnumerator FirstMatchingCharacterSay(Character character, params Entry[] entries)
    {
        return Say(c => character == c, entries);
    }

    public static IEnumerator FirstLiveCharacterSay(params Entry[] entries)
    {
        return Say(c => c.IsAlive, entries);
    }

    private static IEnumerator Say(Predicate<Character> condition, Entry[] entries)
    {
        foreach (var entry in entries)
        {
            if (condition(entry.Character))
            {
                return entry.Say();
            }
        }
        return null;
    }
}