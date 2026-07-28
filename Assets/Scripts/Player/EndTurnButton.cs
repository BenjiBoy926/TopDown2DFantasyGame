using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EndTurnButton : MonoBehaviour
{
    private Button _button;
    private Player _player;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _player = GetComponentInParent<Player>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (_player.IsInputAllowed)
        {
            _player.StartNextTurn();
        }
    }
}