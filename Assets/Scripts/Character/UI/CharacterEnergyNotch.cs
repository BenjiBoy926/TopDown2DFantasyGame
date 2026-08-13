using UnityEngine;

public class CharacterEnergyNotch : MonoBehaviour
{
    public enum State
    {
        Invisible, Filled, Empty, Negative
    }

    [SerializeField] private SpriteRenderer _innerSprite;
    [SerializeField] private Color _filledColor = Color.yellow;
    [SerializeField] private Color _negativeColor = Color.red;

    public void SetState(State state)
    {
        gameObject.SetActive(state != State.Invisible);
        switch (state)
        {
            case State.Filled:
                _innerSprite.color = _filledColor;
                break;
            case State.Empty:
                _innerSprite.color = Color.clear;
                break;
            case State.Negative:
                _innerSprite.color = _negativeColor;
                break;
        }
    }
}