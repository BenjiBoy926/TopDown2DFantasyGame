using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class CharacterSpeechBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Transform _anchor;
    [SerializeField] private float _minWidth = 100;
    [SerializeField] private float _timeBetweenCharacters = 0.05f;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private InputActionReference _proceedAction;
    [SerializeField] private float _repositionDuration = 0.1f;
    [SerializeField] private float _screenPadding = 10;
    private RectTransform _rectTransform;
    private CharacterSpeechBodyText _body;
    private CharacterSpeechBubblePointer _pointer;
    private Vector2 _textPadding;
    private Character _character;
    private bool _proceed;
    private TweenCallback _disableAction;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _body = GetComponentInChildren<CharacterSpeechBodyText>();
        _pointer = GetComponentInChildren<CharacterSpeechBubblePointer>();
        _character = GetComponentInParent<Character>();
        _name.text = _character.Name;
        _disableAction = () => gameObject.SetActive(false);

        _textPadding = _rectTransform.sizeDelta - _body.Size;

        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _proceedAction.action.started += OnProceed;
    }

    private void OnDisable()
    {
        _proceedAction.action.started -= OnProceed;
    }

    private void OnProceed(InputAction.CallbackContext obj)
    {
        _proceed = true;
    }

    public IEnumerator Say(string text)
    {
        gameObject.SetActive(true);

        transform.DOKill();
        transform.DOScale(1, _repositionDuration);
        Reposition(text);
        
        yield return ScrollText(text);
        yield return WaitToProceed();

        transform.DOKill();
        transform.DOScale(0, _repositionDuration).OnComplete(_disableAction);
    }

    private void Reposition(string text)
    {
        _body.Resize(text);

        Vector2 size = _body.Size + _textPadding;
        size.x = Mathf.Max(size.x, _minWidth);
        Vector2 position = GetTargetScreenPosition(size);

        _rectTransform.anchoredPosition = position;
        _rectTransform.DOSizeDelta(size, _repositionDuration);

        Vector2 pointerTarget = GetAnchorScreenPosition();
        _pointer.PointTo(pointerTarget, new(position, size));
    }

    private IEnumerator ScrollText(string text)
    {
        _body.Clear();
        _audioSource.Play();
        _proceed = false;
        for (int i = 0; i < text.Length && !_proceed; i++)
        {
            char c = text[i];
            _body.Append(c);
            yield return YieldToCharacter(c);
        }
        _audioSource.Stop();

        if (_proceed)
        {
            _body.SetText(text);
        }
    }

    private IEnumerator YieldToCharacter(char c)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;
        while (elapsedTime < _timeBetweenCharacters)
        {
            if (_proceed)
            {
                break;
            }
            yield return null;
            elapsedTime = Time.time - startTime;
        }
    }

    private IEnumerator WaitToProceed()
    {
        _proceed = false;
        while (!_proceed)
        {
            yield return null;
        }
    }

    private Vector2 GetTargetScreenPosition(Vector2 speechBubbleSize)
    {
        Vector2 anchorScreenPos = GetAnchorScreenPosition();
        Rect boundingRect = GetPositionRange(speechBubbleSize);
        return Clamp(anchorScreenPos, boundingRect);
    }

    private Rect GetPositionRange(Vector2 speechBubbleSize)
    {
        Rect boundingRect = Rect.MinMaxRect(0, 0, Screen.width, Screen.height);
        Vector2 screenPadding = new(_screenPadding, _screenPadding);
        boundingRect.position += screenPadding;
        boundingRect.size -= screenPadding * 2;
        boundingRect.size -= speechBubbleSize;
        return boundingRect;
    }

    private Vector2 Clamp(Vector2 point, Rect rect)
    {
        point.x = Mathf.Clamp(point.x, rect.xMin, rect.xMax);
        point.y = Mathf.Clamp(point.y, rect.yMin, rect.yMax);
        return point;
    }

    private Vector2 GetAnchorScreenPosition()
    {
        Vector2 worldPos = GetAnchorWorldPosition();
        return _character.WorldToScreen(worldPos);
    }

    private Vector2 GetAnchorWorldPosition()
    {
        return _anchor.position;
    }
}