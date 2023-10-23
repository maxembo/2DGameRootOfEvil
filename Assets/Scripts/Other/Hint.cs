using Scripts.Core.Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    [TextArea(2, 4)]
    [SerializeField] private string _textHint;
    [SerializeField] private Image _dialogBox;
    [SerializeField] private TextMeshProUGUI _dialogText;

    private void OnValidate()
    {
        if (_gameState == null)
            _gameState = FindObjectOfType<GameState>();
    }

    public void HintSetActive(bool value, string text = "")
    {
        _dialogText.text = text;
        _dialogBox.gameObject.SetActive(value);
    }

    #region Trigger

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_gameState.CurrentState == GameStates.Playing && collision.transform.tag == "Player")
            HintSetActive(true, _textHint);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_gameState.CurrentState == GameStates.Playing && collision.transform.tag == "Player")
            HintSetActive(false);
    }

    #endregion
}