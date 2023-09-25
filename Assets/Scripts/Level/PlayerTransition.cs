using UnityEngine;

public class PlayerTransition : MonoBehaviour
{
    [SerializeField] private Transform _placeTransition;

    private Transform _player;
    private GameModes _gameMode;

    private void Start() => EventHandler.OnGameModeChanged.AddListener(ChangeGameMode);

    private void Update()
    {
        if (_player != null && InputData.InputFunctions.GetKeyF_Up())
            Transiting(_player, _placeTransition.transform.position);
    }

    public static void Transiting(Transform obj, Vector2 place) => obj.transform.position = place;

    private void ChangeGameMode(GameModes mode) => _gameMode = mode;

    #region Trigger

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (
            _gameMode == GameModes.Playing &&
            collision.transform.tag == "Player"
            )
            _player = collision.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (
            _gameMode == GameModes.Playing &&
            collision.transform.tag == "Player"
            )
            _player = null;
    }

    #endregion
}