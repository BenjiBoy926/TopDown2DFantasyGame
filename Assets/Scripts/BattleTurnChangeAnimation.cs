using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleTurnChangeAnimation : MonoBehaviour
{
    public bool IsPlaying => _isPlaying;

    [SerializeField] private Image _panel;
    [SerializeField] private TMP_Text _label;
    private bool _isPlaying = false;

    public void Play(Faction faction)
    {
        StopAllCoroutines();
        StartCoroutine(GetPlaySequence(faction));
    }

    private void Awake()
    {
        _panel.enabled = _label.enabled = false;
    }

    private IEnumerator GetPlaySequence(Faction faction)
    {
        _isPlaying = true;
        _panel.enabled = _label.enabled = true;

        Color color = faction.Color;
        color.a = .5f;
        _panel.color = color;
        _label.text = $"{faction.Name} Turn";

        yield return new WaitForSeconds(3);

        _panel.enabled = _label.enabled = false;
        _isPlaying = false;
    }
}
