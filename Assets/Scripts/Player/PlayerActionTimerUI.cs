using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionTimerUI : MonoBehaviour
{
    [Serializable]
    private struct DisplayInfo
    {
        public Sprite Sprite;
        public string Label;
    }

    [SerializeField] private DisplayInfo _undoInfo;
    [SerializeField] private DisplayInfo _redoInfo;
    private Image _image;
    private TMP_Text _label;

    private void Awake()
    {
        _image = GetComponentInChildren<Image>(true);
        _label = GetComponentInChildren<TMP_Text>(true);
        gameObject.SetActive(false);
    }

    public void BeginUndo()
    {
        Begin();
        Display(_undoInfo);
    }

    public void BeginRedo()
    {
        Begin();
        Display(_redoInfo);
    }

    private void Display(DisplayInfo info)
    {
        _image.sprite = info.Sprite;
        _label.text = info.Label;
    }

    private void Begin()
    {
        gameObject.SetActive(true);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }

    public void Confirm()
    {
        gameObject.SetActive(false);
    }
}