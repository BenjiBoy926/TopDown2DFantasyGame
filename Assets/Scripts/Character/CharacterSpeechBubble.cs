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
    [SerializeField] private Vector2 _textPadding;
    [SerializeField] private float _minWidth = 100;
    [SerializeField] private float _timeBetweenCharacters = 0.05f;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private InputActionReference _proceedAction;
    private RectTransform _rectTransform;
    private CharacterSpeechBodyText _body;
    private Character _character;
    private bool _proceed;
    private TweenCallback _disableAction;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _body = GetComponentInChildren<CharacterSpeechBodyText>();
        _character = GetComponentInParent<Character>();
        _name.text = _character.Name;
        _disableAction = () => gameObject.SetActive(false);

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
        _rectTransform.anchoredPosition = GetTargetScreenPosition();

        transform.DOKill();
        transform.DOScale(1, .2f);
        AdjustSize(text);
        
        yield return ScrollText(text);
        yield return WaitToProceed();

        transform.DOKill();
        transform.DOScale(0, .2f).OnComplete(_disableAction);
    }

    private void AdjustSize(string text)
    {
        _body.AdjustSize(text);
        Vector2 size = _body.Size + _textPadding;
        size.x = Mathf.Max(size.x, _minWidth);
        _rectTransform.DOSizeDelta(size, .2f);
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

    private Vector2 GetTargetScreenPosition()
    {
        Vector2 worldPos = GetTargetWorldPosition();
        return _character.WorldToScreen(worldPos);
    }

    private Vector2 GetTargetWorldPosition()
    {
        Vector2 localPosition = _anchor.localPosition;
        if (_character.GetDirection().x < 0)
        {
            localPosition.x *= -1;
        }
        Vector2 worldPosition = _anchor.parent.TransformPoint(localPosition);
        return worldPosition;
    }
}