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
    private bool _skip;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _body = GetComponentInChildren<CharacterSpeechBodyText>();
        _character = GetComponentInParent<Character>();
        _name.text = _character.Name;
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
        _skip = true;
    }

    public IEnumerator Say(string text)
    {
        gameObject.SetActive(true);
        _rectTransform.anchoredPosition = GetTargetScreenPosition();
        AdjustSize(text);

        _body.Clear();
        _audioSource.Play();
        _skip = false;
        for (int i = 0; i < text.Length && !_skip; i++)
        {
            char c = text[i];
            _body.Append(c);
            yield return YieldToCharacter(c);
        }
        _audioSource.Stop();

        if (_skip)
        {
            _body.SetText(text);
        }

        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    private void AdjustSize(string text)
    {
        _body.AdjustSize(text);
        Vector2 size = _body.Size + _textPadding;
        size.x = Mathf.Max(size.x, _minWidth);
        _rectTransform.sizeDelta = size;
    }

    private IEnumerator YieldToCharacter(char c)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;
        while (elapsedTime < _timeBetweenCharacters)
        {
            if (_skip)
            {
                break;
            }
            yield return null;
            elapsedTime = Time.time - startTime;
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