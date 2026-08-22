using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    private Character _character;
    private CharacterHealthUI _healthUI;
    private CharacterPowerUI _powerUI;
    private CharacterEnergyUI _energyUI;
    private bool _isPreviewing = false;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _healthUI = GetComponentInChildren<CharacterHealthUI>();
        _powerUI = GetComponentInChildren<CharacterPowerUI>();
        _energyUI = GetComponentInChildren<CharacterEnergyUI>();
    }

    private void Update()
    {
        if (_isPreviewing)
        {
            Vector2 cellCenter = _character.CurrentCellCenter;
            transform.position = cellCenter;
        }
    }

    public void Preview(int health)
    {
        _healthUI.Preview(health);
        _isPreviewing = true;
    }

    public void ClearPreview()
    {
        _healthUI.ClearPreview();
        _isPreviewing = false;
        transform.localPosition = Vector3.zero;
    }

    public void ShowCurrentHealth()
    {
        _healthUI.ShowCurrentHealth();
    }

    public void AnimateCurrentEnergy()
    {
        _energyUI.AnimateCurrentEnergy();
    }

    public void ShowCurrentEnergy()
    {
        _energyUI.ShowCurrentEnergy();
    }

    public void ShowPower()
    {
        if (_powerUI)
        {
            _powerUI.ShowPower();
        }
    }

    public void ShakeHealthUI()
    {
        _healthUI.Shake();
    }
}