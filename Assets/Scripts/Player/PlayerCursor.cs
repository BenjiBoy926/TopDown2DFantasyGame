using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    [SerializeField] private float _selectionMargin = .25f;
    private PlayerCursorFire _fire;
    private PlayerSelectionAura _aura;
    private Transform _selectionTarget;
    private bool _isVisible = false;
    private bool _isNearTarget = false;

    private void Awake()
    {
        _fire = GetComponentInChildren<PlayerCursorFire>(true);
        _aura = GetComponentInChildren<PlayerSelectionAura>(true);
        ReflectVisibility();
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

    private void Update()
    {
        if (_selectionTarget == null)
            return;

        Vector2 targetPosition = _selectionTarget.position;
        float distance = Vector2.Distance(Position, targetPosition);
        SetIsNearSelectionTarget(distance <= _selectionMargin);
    }

    public void Show()
    {
        SetIsVisible(true);
    }

    public void Hide()
    {
        SetIsVisible(false);
    }

    public void SetSelectionTarget(Transform target)
    {
        _selectionTarget = target;
        if (!_selectionTarget)
        {
            SetIsNearSelectionTarget(false);
        }
    }

    private void SetIsVisible(bool isVisible)
    {
        if (_isVisible == isVisible)
            return;

        _isVisible = isVisible;
        ReflectVisibility();
    }

    private void SetIsNearSelectionTarget(bool isNearTarget)
    {
        if (_isNearTarget == isNearTarget)
            return;

        _isNearTarget = isNearTarget;
        ReflectVisibility();
    }

    private void ReflectVisibility()
    {
        if (!_isVisible)
        {
            _fire.Hide();
            _aura.Hide();
        }
        else if (_isNearTarget)
        {
            _fire.Hide();
            _aura.Show();
        }
        else
        {
            _fire.Show();
            _aura.Hide();
        }
    }
}