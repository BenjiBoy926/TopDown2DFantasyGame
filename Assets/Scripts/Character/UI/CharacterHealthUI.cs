using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class CharacterHealthUI : MonoBehaviour
{
    [SortingLayer, SerializeField] private int _previewLayer;

    [Space]
    [SerializeField] private float _shakeDuration = 0.3f;
    [SerializeField] private float _shakeStrength = 0.5f;
    [SerializeField] private int _shakeVibrato = 100;
    [SerializeField] private float _shakeRandomness = 90;
    [SerializeField] private bool _shakeFadeOut = false;
    [SerializeField] private ShakeRandomnessMode _shakeRandomnessMode = ShakeRandomnessMode.Full;

    private Character _character;
    private CharacterHealthbar _healthBar;
    private CharacterHealthbarPreview _healthBarPreview;
    private CharacterHeartIcon _heartIcon;
    private SortingGroup _sortingGroup;
    private bool _isPreviewing = false;
    private Vector3 _originalLocalPosition;
    private int _originalSortingLayer;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _healthBar = GetComponentInChildren<CharacterHealthbar>();
        _healthBarPreview = GetComponentInChildren<CharacterHealthbarPreview>(true);
        _heartIcon = GetComponentInChildren<CharacterHeartIcon>();
        _sortingGroup = GetComponent<SortingGroup>();
        _originalLocalPosition = transform.localPosition;
        _originalSortingLayer = _sortingGroup.sortingLayerID;
    }

    private void Update()
    {
        if (_isPreviewing)
        {
            Vector2 cellPosition = _character.CurrentCellCenter;
            Vector2 originalWorldPosition = transform.parent.TransformPoint(_originalLocalPosition);
            Vector2 originalWorldOffset = originalWorldPosition - (Vector2)transform.parent.position;
            transform.position = cellPosition - originalWorldOffset;
        }
    }

    public void Preview(int health)
    {
        int higher = Mathf.Max(health, _character.CurrentHealth);
        int lower = Mathf.Min(health, _character.CurrentHealth);
        
        ShowHealth(health);
        _healthBar.ShowHealth(lower);

        _healthBarPreview.Show();
        _healthBarPreview.SetFill(higher);
        _sortingGroup.sortingLayerID = _previewLayer;
        _isPreviewing = true;
    }

    public void ClearPreview()
    {
        _healthBarPreview.Hide();
        ShowCurrentHealth();
        transform.localPosition = _originalLocalPosition;
        _sortingGroup.sortingLayerID = _originalSortingLayer;
        _isPreviewing = false;
    }

    public void ShowCurrentHealth()
    {
        ShowHealth(_character.CurrentHealth);
    }

    public void ShowHealth(int health)
    {
        _healthBar.ShowHealth(health);
        ShowHealthOnHeartIcon(health);
    }

    public void ShowHealthOnHeartIcon(int health)
    {
        int baseHealth = _character.BaseHealth;
        float healthPercentage = (float)health / baseHealth;
        _heartIcon.ShowHealthPercent(healthPercentage);
    }

    public void Shake()
    {
        transform.DOShakePosition(
            _shakeDuration,
            _shakeStrength,
            _shakeVibrato,
            _shakeRandomness,
            false,
            _shakeFadeOut,
            _shakeRandomnessMode);
    }
}