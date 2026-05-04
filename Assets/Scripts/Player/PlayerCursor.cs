using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    // Note: may want to change sprite based on what you are hovering but what I tried before just looked too crowded
}