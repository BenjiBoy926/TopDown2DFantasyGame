using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CharacterDetailPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _healthImage;
    [SerializeField] private TMP_Text _healthLabel;
    [SerializeField] private TMP_Text _powerLabel;
    [SerializeField] private TMP_Text _energyLabel;
    [SerializeField] private TMP_Text _rangeLabel;

    [Space]
    [SerializeField] private float _previewFadeDuration = .35f;
    [SerializeField] private float _shownYPosition = -10;
    [SerializeField] private float _hiddenYPosition = -130;
    [SerializeField] private float _hideShowDuration = .35f;

    private RectTransform _rectTransform;
    private GameObject _allElements;
    private Character _target;

    private void Awake()
    {
        _rectTransform = transform as RectTransform;
        _allElements = transform.GetChild(0).gameObject;
    }

    public void HideImmediately()
    {
        transform.DOKill();
        Vector2 position = _rectTransform.anchoredPosition;
        position.y = _hiddenYPosition;
        _rectTransform.anchoredPosition = position;
    }

    public void Show()
    {
        _rectTransform.DOKill();
        _rectTransform.DOAnchorPosY(_shownYPosition, _hideShowDuration);
    }

    public void Hide()
    {
        _rectTransform.DOKill();
        _rectTransform.DOAnchorPosY(_hiddenYPosition, _hideShowDuration);
    }

    public void Populate(Character character)
    {
        SetTarget(character);
    }

    public void Preview(CharacterInfo info)
    {
        ShowHealth(info.Health);
        ShowEnergy(info.Energy);

        if (info.Health != _target.CurrentHealth)
        {
            _healthLabel.DOFade(0, _previewFadeDuration).SetLoops(-1, LoopType.Yoyo);
        }
        if (info.Energy != _target.CurrentEnergy)
        {
            _energyLabel.DOFade(0, _previewFadeDuration).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void ClearPreview()
    {
        Refresh();
    }

    private void SetTarget(Character target)
    {
        _target = target;
        Refresh();
    }

    private void Refresh()
    {
        _healthLabel.DOKill();
        _energyLabel.DOKill();
        _allElements.SetActive(_target);

        if (_target)
        {
            _nameLabel.text = _target.Name;
            _iconImage.sprite = _target.Icon;

            ShowHealth(_target.CurrentHealth);

            _powerLabel.text = _target.CurrentPower.ToString();
            _powerLabel.color = _target.GetPowerColor(_target.CurrentPower);

            ShowEnergy(_target.CurrentEnergy);

            _rangeLabel.text = _target.TraversalRange.ToString();
        }
        else
        {
            _nameLabel.text = string.Empty;
        }
    }

    private void ShowHealth(int health)
    {
        _healthImage.sprite = _target.GetHeartSprite(health);
        _healthLabel.text = health.ToString();
        _healthLabel.color = _target.GetHealthColor(health);
    }

    private void ShowEnergy(int energy)
    {
        _energyLabel.text = energy.ToString();
        _energyLabel.color = _target.GetEnergyColor(energy);
    }
}