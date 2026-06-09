using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    private PlayerCursorFire _fire;

    private void Awake()
    {
        _fire = GetComponentInChildren<PlayerCursorFire>(true);
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            Cursor.visible = false;
        }
    }

    public void Show()
    {
        _fire.Show();
    }

    public void Hide()
    {
        _fire.Hide();
    }
}