using UnityEngine;

public class BattleRecord : MonoBehaviour
{
    public void RecordInitialState()
    {
        Debug.Log("Recorded initial state");
    }

    public void Record(Character a, Character b)
    {
        Debug.Log($"Recorded action betwee {a.name} and {b.name}");
    }

    public void Record(Character character)
    {
        Debug.Log($"Recorded action for {character.name}");
    }

    public void Undo()
    {
        Debug.Log("Undo!");
    }

    public void Redo()
    {
        Debug.Log("Redo!");
    }
}