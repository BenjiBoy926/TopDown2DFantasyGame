using UnityEngine;

public class PlayerCursorFire : MonoBehaviour
{
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private ParticleSystem _burst;

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