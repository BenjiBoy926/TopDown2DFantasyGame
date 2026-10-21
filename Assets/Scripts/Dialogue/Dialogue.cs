using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
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

        public Coroutine Say()
        {
            return _character.Say(_line);
        }
    }

    public static Coroutine FirstMatchingMainCharacterSay(Character character, params string[] lines)
    {
        Entry[] entries = new Entry[lines.Length];
        for (int i = 0; i < lines.Length && i < MainCharacters.Count; i++)
        {
            entries[i] = new(MainCharacters[i], lines[i]);
        }
        return FirstMatchingCharacterSay(character, entries);
    }

    public static Coroutine FirstMatchingCharacterSay(Character character, params Entry[] entries)
    {
        return Say(c => character == c, entries);
    }

    public static Coroutine FirstLiveMainCharacterSay(params string[] lines)
    {
        Entry[] entries = new Entry[lines.Length];
        for (int i = 0; i < lines.Length && i < MainCharacters.Count; i++)
        {
            entries[i] = new(MainCharacters[i], lines[i]);
        }
        return FirstLiveCharacterSay(entries);
    }

    public static Coroutine FirstLiveCharacterSay(params Entry[] entries)
    {
        return Say(c => c.IsAlive, entries);
    }

    public static Coroutine Say(Predicate<Character> condition, params Entry[] entries)
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