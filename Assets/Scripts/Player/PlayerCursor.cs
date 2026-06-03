using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private ParticleSystem _burst;

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
        _burst.Play();
        _fire.Play();
    }

    public void Hide()
    {
        _burst.Play();
        _fire.Stop();
    }
}