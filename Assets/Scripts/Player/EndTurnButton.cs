using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EndTurnButton : MonoBehaviour
{
    private Button _button;
    private Battle _battle;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _battle = GetComponentInParent<Battle>();
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
        // TODO: link this to the Player script in some way
        // so that you can only end the turn if Player._isInputAllowed
        _battle.StartNextTurn();
    }
}