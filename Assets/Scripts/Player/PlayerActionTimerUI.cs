using UnityEngine;

public class PlayerActionTimerUI : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void BeginUndo()
    {
        Begin();
    }

    public void BeginRedo()
    {
        Begin();
    }

    private void Begin()
    {
        gameObject.SetActive(true);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }

    public void Confirm()
    {
        gameObject.SetActive(false);
    }
}