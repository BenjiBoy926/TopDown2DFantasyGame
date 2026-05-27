using UnityEngine;

public class SpriteSlider : MonoBehaviour
{
    public float Value
    {
        get => _value;
        set
        {
            _value = Mathf.Clamp01(value);
            UpdateFill();
        }
    }
    private float _value;

    private void UpdateFill()
    {
        Vector3 scale = transform.localScale;
        scale.x = _value;
        transform.localScale = scale;
    }
}