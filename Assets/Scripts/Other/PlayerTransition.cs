using Scripts.Core.Global;
using UnityEngine;

public class PlayerTransition : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private Transform _placeTransition;

    private Transform _player;
    
    private void OnValidate()
    {
        if (_gameState == null)
            _gameState = FindObjectOfType<GameState>();
    }

    private void Update()////make event
    {
        if (_player != null && InputFunctions.GetKeyF_Up())
            Transiting(_player, _placeTransition.transform.position);
    }

    public static void Transiting(Transform obj, Vector2 place) => obj.transform.position = place;

    #region Trigger

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CheckConditions(collision.transform))
            _player = collision.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CheckConditions(collision.transform))
            _player = null;
    }

    private bool CheckConditions(Transform t) => 
        _gameState.CurrentState == GameStates.Playing && t.CompareTag("Player");
    
    #endregion
}