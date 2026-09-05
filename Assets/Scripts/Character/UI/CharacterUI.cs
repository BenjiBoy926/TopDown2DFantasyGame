using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class CharacterUI : MonoBehaviour
{
    [SerializeField, SortingLayer] private int _previewSortingLayer;

    private Character _character;
    private CharacterHealthUI _healthUI;
    private CharacterPowerUI _powerUI;
    private CharacterEnergyUI _energyUI;
    
    private SortingGroup _sortingGroup;
    private int _defaultSortingLayer;
    private bool _isPreviewing = false;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _healthUI = GetComponentInChildren<CharacterHealthUI>();
        _powerUI = GetComponentInChildren<CharacterPowerUI>();
        _energyUI = GetComponentInChildren<CharacterEnergyUI>();

        _sortingGroup = GetComponent<SortingGroup>();
        _defaultSortingLayer = _sortingGroup.sortingLayerID;
    }

    private void Update()
    {
        if (_isPreviewing)
        {
            Vector2 cellCenter = _character.CurrentCellCenter;
            transform.position = cellCenter;
        }
    }

    public void Show()
    {
        transform.DOKill();
        transform.DOScaleX(1, .35f);
    }

    public void Hide()
    {
        transform.DOKill();
        transform.DOScaleX(0, .35f);
    }

    public void Preview(CharacterInfo info)
    {
        _healthUI.Preview(info);
        _energyUI.Preview(info);
        _sortingGroup.sortingLayerID = _previewSortingLayer;
        _isPreviewing = true;
    }

    public void ClearPreview()
    {
        _healthUI.ClearPreview();
        _energyUI.ClearPreview();
        _isPreviewing = false;
        transform.localPosition = Vector3.zero;
        _sortingGroup.sortingLayerID = _defaultSortingLayer;
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